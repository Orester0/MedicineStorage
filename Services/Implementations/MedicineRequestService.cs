using MedicineStorage.Models.MedicineModels;
using MedicineStorage.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace MedicineStorage.Services.Implementations
{
    public class MedicineRequestService(IMedicineRequestRepository _medicineRequestRepository, IStockRepository _stockRepository) : IMedicineRequestService
    {

        public async void CreateMedicineRequest(MedicineRequest request)
        {
            if (request.Medicine.RequiresSpecialApproval)
            {
                //
            }

            await _medicineRequestRepository.AddAsync(request);
        }

        public async Task<bool> ApproveRequest(int requestId, bool isAdmin = false)
        {
            var request = await _medicineRequestRepository.GetByIdAsync(requestId);
            if (request == null || request.Status == RequestStatus.Approved) return false;

            if (request.Medicine.RequiresSpecialApproval && !isAdmin)
            {
                throw new InvalidOperationException("Requires additional admin approval");
            }

            //
            await _medicineRequestRepository.Update(request);

            //DeductFromStock(request);
            return true;
        }
        /*
        private void DeductFromStock(MedicineRequest request)
        {
            var stock = _stockRepository.GetByCondition(s => s.MedicineId == request.MedicineId && s.Quantity >= request.Quantity);
            if (stock != null)
            {
                stock.Quantity -= request.Quantity;
                _stockRepository.Update(stock);
            }
            else
            {
                throw new InvalidOperationException("Insufficient stock");
            }
        }*/
    }
}
