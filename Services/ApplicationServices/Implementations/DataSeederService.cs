using MedicineStorage.Models.DTOs;
using MedicineStorage.Models.MedicineModels;
using MedicineStorage.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using MedicineStorage.Data.Interfaces;
using MedicineStorage.Services.BusinessServices.Interfaces;
using MedicineStorage.Models.UserModels;
using MedicineStorage.Services.ApplicationServices.Interfaces;

namespace MedicineStorage.Services.ApplicationServices.Implementations
{
    public class DataSeederService(
        IUnitOfWork _unitOfWork,
        IMapper _mapper,
        IUserService _userService,
        IMedicineRequestService _medicineRequestService) : IDataSeederService
    {
        public async Task<ServiceResult<List<User>>> GenerateRandomUsersAsync()
        {
            var result = new ServiceResult<List<User>>();
            var createdUsers = new List<User>();

            var firstNames = new[] { "John", "Sarah", "Michael", "Emma", "David", "Anna", "Robert", "Lisa", "William", "Maria" };
            var lastNames = new[] { "Smith", "Johnson", "Brown", "Jones", "Miller", "Davis", "Wilson", "Moore", "Taylor", "Anderson" };
            var positions = new[] { "Cardiologist", "Neurologist", "Pediatrician", "Surgeon", "Dentist", "Therapist", "Ophthalmologist", "Psychiatrist" };
            var companies = new[] { "City Hospital", "Medical Center", "Health Clinic", "Regional Hospital", "Children's Hospital" };

            var random = new Random();

            for (int i = 0; i < 17; i++)
            {
                var firstName = firstNames[random.Next(firstNames.Length)];
                var lastName = lastNames[random.Next(lastNames.Length)];

                var registerDto = new UserRegistrationDTO
                {
                    FirstName = firstName,
                    LastName = lastName,
                    UserName = $"{firstName.ToLower()}.{lastName.ToLower()}{random.Next(100)}",
                    Email = $"{firstName.ToLower()}.{lastName.ToLower()}{random.Next(100)}@example.com",
                    Password = "ZXCZXC1",
                    Position = positions[random.Next(positions.Length)],
                    Company = companies[random.Next(companies.Length)],
                    Roles = new List<string> { "Doctor" }
                };

                var userResult = await _userService.CreateUserAsync(registerDto);
                if (!userResult.Success)
                {
                    result.Errors.AddRange(userResult.Errors);
                    continue;
                }

                createdUsers.Add(userResult.Data);
            }

            for (int i = 0; i < 3; i++)
            {
                var firstName = firstNames[random.Next(firstNames.Length)];
                var lastName = lastNames[random.Next(lastNames.Length)];

                var registerDto = new UserRegistrationDTO
                {
                    FirstName = firstName,
                    LastName = lastName,
                    UserName = $"{firstName.ToLower()}.{lastName.ToLower()}{random.Next(100)}",
                    Email = $"{firstName.ToLower()}.{lastName.ToLower()}{random.Next(100)}@example.com",
                    Password = "ZXCZXC1",
                    Position = "Pharmacy Manager",
                    Company = companies[random.Next(companies.Length)],
                    Roles = new List<string> { "Manager" }
                };

                var userResult = await _userService.CreateUserAsync(registerDto);
                if (!userResult.Success)
                {
                    result.Errors.AddRange(userResult.Errors);
                    continue;
                }

                createdUsers.Add(userResult.Data);
            }

            result.Data = createdUsers;
            return result;
        }

        public async Task<ServiceResult<List<ReturnMedicineRequestDTO>>> GenerateRandomMedicineRequestsAsync()
        {
            var result = new ServiceResult<List<ReturnMedicineRequestDTO>>();
            var createdRequests = new List<ReturnMedicineRequestDTO>();

            var medicines = await _unitOfWork.MedicineRepository.GetAllAsync();
            if (!medicines.Any())
            {
                result.Errors.Add("No medicines found in the database");
                return result;
            }

            var doctorUsers = await _unitOfWork.UserRepository.GetUsersByRoleAsync("Doctor");
            if (!doctorUsers.Any())
            {
                result.Errors.Add("No doctor users found in the database");
                return result;
            }

            var random = new Random();
            var justifications = new[]
            {
                "Needed for emergency department",
                "Regular stock replenishment",
                "Anticipated increase in patient needs",
                "Current stock expires soon",
                "Required for scheduled surgeries",
                "Preparing for seasonal conditions",
                "Research protocol requirements",
                "New treatment procedures implementation"
            };

            foreach (var doctor in doctorUsers)
            {
                int requestsCount = random.Next(5, 25);

                for (int i = 0; i < requestsCount; i++)
                {
                    var medicine = medicines[random.Next(medicines.Count)];

                    var requiredByDate = DateTime.UtcNow.AddDays(random.Next(1, 31));

                    var requestDto = new CreateMedicineRequestDTO
                    {
                        MedicineId = medicine.Id,
                        Quantity = Convert.ToDecimal(random.Next(13, 69)),
                        RequiredByDate = requiredByDate,
                        Justification = justifications[random.Next(justifications.Length)]
                    };

                    var requestResult = await _medicineRequestService.CreateRequestAsync(requestDto, doctor.Id);
                    if (!requestResult.Success)
                    {
                        result.Errors.AddRange(requestResult.Errors);
                        continue;
                    }

                    createdRequests.Add(requestResult.Data);
                }
            }

            result.Data = createdRequests;
            return result;
        }
        public async Task<BulkOperationResult> BulkCreateMedicinesAsync(List<BulkCreateMedicineDTO> medicinesList)
        {
            var result = new BulkOperationResult();
            var categoriesToCreate = new Dictionary<string, MedicineCategory>();
            var medicinesToCreate = new List<(Medicine medicine, int initialStock)>();
            var suppliesToCreate = new List<MedicineSupply>();

            foreach (var item in medicinesList)
            {
                try
                {
                    var categoryName = item.Medicine.Category;
                    if (!categoriesToCreate.ContainsKey(categoryName))
                    {
                        await _unitOfWork.BeginTransactionAsync();
                        var category = await _unitOfWork.MedicineRepository.GetOrCreateCategoryAsync(categoryName);
                        await _unitOfWork.CompleteAsync();
                        await _unitOfWork.CommitTransactionAsync();

                        categoriesToCreate.Add(categoryName, category);
                    }
                }
                catch (DbUpdateException ex) when (IsDuplicateKeyViolation(ex))
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    result.AddFailure(item.Medicine.Name, $"Duplicate category name: {item.Medicine.Category}");
                    categoriesToCreate[item.Medicine.Category] = null;
                }
                catch (Exception ex)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    result.AddFailure(item.Medicine.Name, $"Failed to create category: {ex.Message}");
                    categoriesToCreate[item.Medicine.Category] = null;
                }
            }

            foreach (var item in medicinesList)
            {
                try
                {
                    var category = categoriesToCreate[item.Medicine.Category];
                    if (category == null)
                    {
                        continue;
                    }

                    await _unitOfWork.BeginTransactionAsync();

                    var medicine = _mapper.Map<Medicine>(item.Medicine);
                    medicine.Stock = item.InitialStock;
                    medicine.LastAuditDate = null;
                    medicine.CategoryId = category.Id;

                    await _unitOfWork.MedicineRepository.AddAsync(medicine);
                    await _unitOfWork.CompleteAsync();
                    await _unitOfWork.CommitTransactionAsync();

                    medicinesToCreate.Add((medicine, (int)item.InitialStock));
                }
                catch (DbUpdateException ex) when (IsDuplicateKeyViolation(ex))
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    result.AddFailure(item.Medicine.Name, "Duplicate medicine name");
                }
                catch (Exception ex)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    result.AddFailure(item.Medicine.Name, $"Failed to create medicine: {ex.Message}");
                }
            }

            foreach (var (medicine, initialStock) in medicinesToCreate)
            {
                try
                {
                    await _unitOfWork.BeginTransactionAsync();

                    var supply = new MedicineSupply
                    {
                        MedicineId = medicine.Id,
                        Quantity = initialStock,
                        TransactionDate = DateTime.UtcNow
                    };

                    await _unitOfWork.MedicineSupplyRepository.AddAsync(supply);

                    await _unitOfWork.CompleteAsync();
                    await _unitOfWork.CommitTransactionAsync();

                    result.SuccessCount++;
                }
                catch (Exception ex)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    result.AddFailure(medicine.Name, $"Failed to create supply: {ex.Message}");
                }
            }

            var usersResult = await GenerateRandomUsersAsync();
            if (!usersResult.Success)
            {
                throw new Exception($"Failed to generate data.");
            }

            var requestsResult = await GenerateRandomMedicineRequestsAsync();
            if (!requestsResult.Success)
            {
                throw new Exception($"Failed to generate data.");
            }

            Console.WriteLine("EndOfInsert");
            return result;
        }



        private bool IsDuplicateKeyViolation(DbUpdateException exception)
        {
            return exception.InnerException is SqlException sqlEx &&
                    (sqlEx.Number == 2601 || sqlEx.Number == 2627);
        }


    }
}
