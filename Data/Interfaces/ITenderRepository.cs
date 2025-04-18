﻿using MedicineStorage.Models.Params;
using MedicineStorage.Models.TenderModels;
using Microsoft.EntityFrameworkCore;

namespace MedicineStorage.Data.Interfaces
{
    public interface ITenderRepository : IGenericRepository<Tender>
    {

        Task<IEnumerable<Tender>> GetByMedicineIdAndDateRangeAsync(int medicineId, DateTime startDate, DateTime endDate);
        Task<(IEnumerable<Tender>, int)> GetByParams(TenderParams tenderParams);
        Task<IEnumerable<Tender>> GetTendersCreatedByUserIdAsync(int userId);
        Task<IEnumerable<Tender>> GetTendersAwardedByUserIdAsync(int userId);
        Task<Tender> GetTenderByProposalIdAsync(int proposalId);
        Task<IEnumerable<Tender>> GetPublishedTendersAsync();
        Task<IEnumerable<Tender>> GetRelevantTendersAsync();
    }
}
