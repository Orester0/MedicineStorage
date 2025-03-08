using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using MedicineStorage.Services.BusinessServices.Interfaces;

namespace MedicineStorage.Services.BusinessServices.Implementations
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly BlobContainerClient _containerClient;

        public BlobStorageService(IConfiguration configuration)
        {
            var connectionString = configuration["AzureStorage:ConnectionString"] 
                ?? throw new ArgumentNullException("Azure Storage connection string is not configured");
            var containerName = configuration["AzureStorage:PhotosContainer"] 
                ?? throw new ArgumentNullException("Azure Storage container name is not configured");
            
            _containerClient = new BlobContainerClient(connectionString, containerName);
        }

        public async Task<string> UploadPhotoAsync(byte[] photoData, string fileName)
        {
            var blobName = $"{Guid.NewGuid()}-{fileName}";
            var blobClient = _containerClient.GetBlobClient(blobName);

            using var stream = new MemoryStream(photoData);
            await blobClient.UploadAsync(stream, new BlobUploadOptions
            {
                HttpHeaders = new BlobHttpHeaders { ContentType = "image/jpeg" }
            });

            return blobName;
        }

        public async Task<byte[]> DownloadPhotoAsync(string blobName)
        {
            var blobClient = _containerClient.GetBlobClient(blobName);
            
            if (!await blobClient.ExistsAsync())
            {
                throw new KeyNotFoundException($"Photo with name {blobName} not found");
            }

            using var memoryStream = new MemoryStream();
            await blobClient.DownloadToAsync(memoryStream);
            return memoryStream.ToArray();
        }

        public async Task DeletePhotoAsync(string blobName)
        {
            var blobClient = _containerClient.GetBlobClient(blobName);
            await blobClient.DeleteIfExistsAsync();
        }
    }
} 