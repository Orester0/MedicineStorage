using CloudinaryDotNet.Actions;
using MedicineStorage.Services.ApplicationServices.Implementations;
using Microsoft.Extensions.Options;

namespace MedicineStorage.Services.ApplicationServices.Interfaces
{
    public interface IPhotoService
    {
        public Task<ImageUploadResult> AddPhotoAsync(IFormFile file);

        public Task<DeletionResult> DeletePhotoAsync(string publicId);
    }
}
