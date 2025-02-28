using MedicineStorage.Data.Interfaces;
using MedicineStorage.Models.TenderModels;
using MedicineStorage.Services.ApplicationServices.Interfaces;

namespace MedicineStorage.Services.ApplicationServices.Implementations
{
    public class DeadlineDateCheckService(IUnitOfWork _unitOfWork) : IDeadlineDateCheckService
    {
        public async Task CheckAndCloseExpiredTendersAsync()
        {
            var publishedTenders = await _unitOfWork.TenderRepository.GetPublishedTendersAsync();
            var expiredTenders = publishedTenders.Where(t => t.DeadlineDate < DateTime.UtcNow);

            foreach (var tender in expiredTenders)
            {
                tender.Status = TenderStatus.Closed;
                tender.ClosingDate = DateTime.UtcNow;
                _unitOfWork.TenderRepository.Update(tender);
            }

            await _unitOfWork.CompleteAsync();
        }
    }
}
