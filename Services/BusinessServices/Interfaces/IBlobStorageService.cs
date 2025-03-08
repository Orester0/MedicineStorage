using Microsoft.AspNetCore.Http;

namespace MedicineStorage.Services.BusinessServices.Interfaces
{
    public interface IBlobStorageService
    {
        Task<string> UploadPhotoAsync(byte[] photoData, string fileName);
        Task<byte[]> DownloadPhotoAsync(string blobName);
        Task DeletePhotoAsync(string blobName);
    }
} 