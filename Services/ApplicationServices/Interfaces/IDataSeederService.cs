using MedicineStorage.Models.DTOs;
using MedicineStorage.Models;

namespace MedicineStorage.Services.ApplicationServices.Interfaces
{
    public interface IDataSeederService
    {
        Task<BulkOperationResult> BulkCreateMedicinesAsync(List<BulkCreateMedicineDTO> medicinesList);
    }
}
