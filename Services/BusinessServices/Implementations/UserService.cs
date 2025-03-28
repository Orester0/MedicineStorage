using AutoMapper;
using Azure.Messaging.ServiceBus;
using MedicineStorage.Data.Interfaces;
using MedicineStorage.Helpers;
using MedicineStorage.Models;
using MedicineStorage.Models.DTOs;
using MedicineStorage.Models.Params;
using MedicineStorage.Models.UserModels;
using MedicineStorage.Services.BusinessServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json.Linq;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using System.Text.Json;
using User = MedicineStorage.Models.UserModels.User;

namespace MedicineStorage.Services.BusinessServices.Implementations
{
    public class UserService(
        IUnitOfWork _unitOfWork,
        IMapper _mapper,
        IBlobStorageService _blobStorageService,
        IConfiguration _configuration,
        CosmosClient _cosmosClient) : IUserService
    {

        private readonly string DEFAULT_PHOTO_BLOB_NAME =
            _configuration["AzureStorage:DefaultImagePath"]
            ?? throw new Exception("Default image path cannot be null.");

        private readonly string COSMOS_DATABASE_NAME =
            _configuration["CosmosDb:DatabaseName"]
            ?? throw new Exception("Default image path cannot be null.");

        private readonly string COSMOS_USER_CONNECTIONS_CONTAINER_NAME =
            _configuration["CosmosDb:UserConnectionsContainerName"]
            ?? throw new Exception("Default image path cannot be null.");


        private readonly string SERVICE_BUS_CONNECTION_STRING =
            _configuration["ServiceBus:ConnectionString"]
            ?? throw new Exception("Default image path cannot be null.");


        private readonly string SERVICE_BUS_USERS_QUEUE_NAME =
            _configuration["ServiceBus:UsersQueueName"]
            ?? throw new Exception("Default image path cannot be null.");


        private async Task SendMessageToServiceBus(User user)
        {

            await using var client = new ServiceBusClient(SERVICE_BUS_CONNECTION_STRING);
            ServiceBusSender sender = client.CreateSender(SERVICE_BUS_USERS_QUEUE_NAME);

            var messageBody = new
            {
                userId = user.Id,
                firstName = user.FirstName,
                lastName = user.LastName,
                position = user.Position,
                company = user.Company,
                timestamp = DateTime.UtcNow.ToString("o")
            };

            string messageJson = JsonSerializer.Serialize(messageBody);
            ServiceBusMessage message = new ServiceBusMessage(messageJson);

            await sender.SendMessageAsync(message);
        }


        private async Task AddUserToCosmosDB(User user)
        {
            var connectionTimestamp = DateTime.UtcNow.ToString("o");
            var container = _cosmosClient.GetContainer(COSMOS_DATABASE_NAME, COSMOS_USER_CONNECTIONS_CONTAINER_NAME);
            try
            {
                var response = await container.ReadItemAsync<dynamic>(
                    user.Id.ToString(),
                    new PartitionKey(user.Id));


                var existingDocument = response.Resource;

                var connections = (existingDocument.connections as JArray)?.ToObject<List<dynamic>>() ?? new List<dynamic>();

                connections.Add(new { timestamp = connectionTimestamp });

                existingDocument.connections = JArray.FromObject(connections);

                await container.ReplaceItemAsync(existingDocument, user.Id.ToString(), new PartitionKey(user.Id));

            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                var userDto = new
                {
                    id = user.Id.ToString(),
                    userId = user.Id,
                    firstName = user.FirstName,
                    lastName = user.LastName,
                    position = user.Position,
                    company = user.Company,
                    connections = new[]
                        {
                        new { timestamp = connectionTimestamp
                        }
                    }
                };

                await container.CreateItemAsync(userDto, new PartitionKey(user.Id));

            }
            catch (CosmosException ex)
            {
                throw new Exception($"Error accessing Cosmos DB: {ex.Message}", ex);
            }

        }

        public async Task<User> LoginUser(UserLoginDTO loginRequest)
        {
            var user = await GetByUserNameAsync(loginRequest.UserName);

            if (!user.Success || user.Data == null)
            {
                throw new UnauthorizedAccessException("Invalid Username");
            }
            if (!await VerifyPasswordAsync(user.Data, loginRequest.Password))
            {
                throw new UnauthorizedAccessException("Invalid Password");
            }

            await SendMessageToServiceBus(user.Data);
            return user.Data;
        }
       




        private async Task<string?> GetBase64PhotoAsync(string? blobName)
        {
            if (string.IsNullOrEmpty(blobName))
                return null;

            try
            {
                var photoBytes = await _blobStorageService.DownloadPhotoAsync(blobName);
                return $"data:image/jpeg;base64,{Convert.ToBase64String(photoBytes)}";
            }
            catch
            {
                return null;
            }
        }

        public async Task<ServiceResult<User>> RegisterUser(UserRegistrationDTO registerDto)
        {
            var result = new ServiceResult<User>();

            if (await UserExists(registerDto.UserName))
            {
                result.Errors.Add($"Username '{registerDto.UserName}' is taken");
            }

            if (await EmailTaken(registerDto.Email))
            {
                result.Errors.Add($"Email '{registerDto.Email}' is taken");
            }

            foreach (var role in registerDto.Roles)
            {
                if (role is "Manager" or "Admin")
                {
                    result.Errors.Add($"Cannot register as '{role}'");
                }

                if (!await RoleExistsAsync(role))
                {
                    result.Errors.Add($"Role '{role}' does not exist");
                }
            }

            if (result.Errors.Any())
            {
                return result;
            }

            var user =  await CreateUserAsync(registerDto);
            await SendMessageToServiceBus(user.Data);
            return user;
        }

        public async Task<ServiceResult<User>> CreateUserAsync(UserRegistrationDTO registerDto)
        {
            var result = new ServiceResult<User>();
            var user = _mapper.Map<User>(registerDto);

            user.PhotoBlobName = DEFAULT_PHOTO_BLOB_NAME;

            var (createResult, createdUser) = await _unitOfWork.UserRepository.CreateUserAsync(user, registerDto.Password);
            if (!createResult.Succeeded)
            {
                result.Errors.AddRange(createResult.Errors.Select(e => e.Description));
                return result;
            }

            var roleResult = await _unitOfWork.UserRepository.AddToRolesAsync(createdUser, registerDto.Roles);
            if (!roleResult.Succeeded)
            {
                result.Errors.AddRange(roleResult.Errors.Select(e => e.Description));
                return result;
            }

            await _unitOfWork.CompleteAsync();
            result.Data = createdUser;
            return result;
        }

        public async Task<ServiceResult<List<ReturnUserGeneralDTO>>> GetAllAsync()
        {
            var result = new ServiceResult<List<ReturnUserGeneralDTO>>();
            var users = await _unitOfWork.UserRepository.GetUsersWithRolesAsync();
            
            var dtos = new List<ReturnUserGeneralDTO>();
            foreach (var user in users)
            {
                var dto = _mapper.Map<ReturnUserGeneralDTO>(user);
                dto.PhotoBase64 = await GetBase64PhotoAsync(user.PhotoBlobName);
                dtos.Add(dto);
            }
            
            result.Data = dtos;
            return result;
        }

        public async Task<ServiceResult<PagedList<ReturnUserPersonalDTO>>> GetPaginatedUsers(UserParams parameters)
        {
            var result = new ServiceResult<PagedList<ReturnUserPersonalDTO>>();
            var (users, totalCount) = await _unitOfWork.UserRepository.GetPaginatedUsersAsync(parameters);
            
            var dtos = new List<ReturnUserPersonalDTO>();
            foreach (var user in users)
            {
                var dto = _mapper.Map<ReturnUserPersonalDTO>(user);
                dto.PhotoBase64 = await GetBase64PhotoAsync(user.PhotoBlobName);
                dtos.Add(dto);
            }
            
            result.Data = new PagedList<ReturnUserPersonalDTO>(dtos, totalCount, parameters.PageNumber, parameters.PageSize);
            return result;
        }

        public async Task UploadPhotoAsync(IFormFile file, int userId)
        {
            var user = await _unitOfWork.UserRepository.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                throw new UnauthorizedAccessException("Unauthorized");
            }

            // Delete old photo if exists and it's not the default photo
            if (!string.IsNullOrEmpty(user.PhotoBlobName) && user.PhotoBlobName != DEFAULT_PHOTO_BLOB_NAME)
            {
                await _blobStorageService.DeletePhotoAsync(user.PhotoBlobName);
            }

            byte[] compressedImage = await CompressImageAsync(file);
            user.PhotoBlobName = await _blobStorageService.UploadPhotoAsync(compressedImage, file.FileName);

            await _unitOfWork.UserRepository.UpdateAsync(user);
            await _unitOfWork.CompleteAsync();
        }

        private async Task<byte[]> CompressImageAsync(IFormFile file)
        {
            using (var stream = file.OpenReadStream())
            using (var image = await Image.LoadAsync(stream))
            {
                image.Mutate(x => x.Resize(128, 128));

                using (var ms = new MemoryStream())
                {
                    await image.SaveAsync(ms, new JpegEncoder { Quality = 85 });
                    return ms.ToArray();
                }
            }
        }

        public async Task<ServiceResult<User>> GetByIdAsync(int id)
        {
            var result = new ServiceResult<User>();
            var user = await _unitOfWork.UserRepository.GetUserByIdWithRolesAsync(id);

            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            result.Data = user;
            return result;
        }

        public async Task<ServiceResult<User>> GetByUserNameAsync(string username)
        {
            var result = new ServiceResult<User>();
            var user = await _unitOfWork.UserRepository.GetByUserNameWithRolesAsync(username);

            if (user == null)
            {
                result.Errors.Add("User not found.");
                return result;
            }

            result.Data = user;
            return result;
        }

        public async Task<ServiceResult<ReturnUserPersonalDTO>> GetPersonalUserByIdAsync(int id)
        {
            var result = new ServiceResult<ReturnUserPersonalDTO>();
            var user = await _unitOfWork.UserRepository.GetUserByIdWithRolesAsync(id);

            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            var dto = _mapper.Map<ReturnUserPersonalDTO>(user);
            dto.PhotoBase64 = await GetBase64PhotoAsync(user.PhotoBlobName);

            result.Data = dto;
            return result;
        }

        public async Task<ServiceResult<bool>> UpdateUserAsync(UserUpdateDTO updateDto, int userId)
        {
            var result = new ServiceResult<bool>();
            var existingUser = await _unitOfWork.UserRepository.FindByIdAsync(userId.ToString());

            if (existingUser == null)
            {
                result.Errors.Add("User not found");
                return result;
            }

            _mapper.Map(updateDto, existingUser);
            var updateResult = await _unitOfWork.UserRepository.UpdateAsync(existingUser);

            if (!updateResult.Succeeded)
            {
                result.Errors.AddRange(updateResult.Errors.Select(e => e.Description));
                return result;
            }

            await _unitOfWork.CompleteAsync();
            result.Data = true;
            return result;
        }

        public async Task<ServiceResult<bool>> DeleteUserAsync(int id)
        {
            var result = new ServiceResult<bool>();
            var user = await _unitOfWork.UserRepository.FindByIdAsync(id.ToString());

            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            var deleteResult = await _unitOfWork.UserRepository.DeleteAsync(user);
            if (!deleteResult.Succeeded)
            {
                result.Errors.AddRange(deleteResult.Errors.Select(e => e.Description));
                return result;
            }

            await _unitOfWork.CompleteAsync();
            result.Data = true;
            return result;
        }

        public async Task<ServiceResult<bool>> UpdateRolesAsync(int userId, List<string> roleNames)
        {
            var result = new ServiceResult<bool>();
            var user = await _unitOfWork.UserRepository.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            var allRoles = await _unitOfWork.UserRepository.GetAllRoleNamesAsync();
            var invalidRoles = roleNames.Except(allRoles).ToList();

            if (invalidRoles.Any())
            {
                result.Errors.Add($"Invalid roles: {string.Join(", ", invalidRoles)}");
                return result;
            }

            var currentRoles = await _unitOfWork.UserRepository.GetUserRolesAsync(user);
            var rolesToRemove = currentRoles.Except(roleNames).ToList();
            var rolesToAdd = roleNames.Except(currentRoles).ToList();

            if (rolesToRemove.Any())
            {
                var removeResult = await _unitOfWork.UserRepository.RemoveFromRolesAsync(user, rolesToRemove);
                if (!removeResult.Succeeded)
                {
                    result.Errors.AddRange(removeResult.Errors.Select(e => e.Description));
                    return result;
                }
            }

            if (rolesToAdd.Any())
            {
                var addResult = await _unitOfWork.UserRepository.AddToRolesAsync(user, rolesToAdd);
                if (!addResult.Succeeded)
                {
                    result.Errors.AddRange(addResult.Errors.Select(e => e.Description));
                    return result;
                }
            }

            await _unitOfWork.CompleteAsync();
            result.Data = true;
            return result;
        }

        public async Task<ServiceResult<bool>> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
        {
            var result = new ServiceResult<bool>();
            var user = await _unitOfWork.UserRepository.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {userId} not found.");
            }

            var passwordResult = await _unitOfWork.UserRepository.ChangePasswordAsync(user, currentPassword, newPassword);
            if (!passwordResult.Succeeded)
            {
                result.Errors.AddRange(passwordResult.Errors.Select(e => e.Description));
                return result;
            }

            await _unitOfWork.CompleteAsync();
            result.Data = true;
            return result;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _unitOfWork.UserRepository.GetUserByEmailWithRolesAsync(email);
        }

        public async Task<bool> VerifyPasswordAsync(User user, string password)
        {
            return await _unitOfWork.UserRepository.CheckPasswordAsync(user, password);
        }

        public async Task<bool> RoleExistsAsync(string roleName)
        {
            return await _unitOfWork.UserRepository.RoleExistsAsync(roleName);
        }

        public async Task<bool> UserExists(string login)
        {
            return await _unitOfWork.UserRepository.UserExistsAsync(login);
        }

        public async Task<bool> EmailTaken(string email)
        {
            return await _unitOfWork.UserRepository.EmailTakenAsync(email);
        }
    }
}

