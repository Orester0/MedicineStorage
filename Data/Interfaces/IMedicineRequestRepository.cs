﻿using MedicineStorage.Models.MedicineModels;
using MedicineStorage.Models.TenderModels;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MedicineStorage.Data.Interfaces
{
    public interface IMedicineRequestRepository
    {

        public Task<MedicineRequest?> GetByIdAsync(int id);

        public Task<List<MedicineRequest>> GetAllAsync();

        public Task<List<MedicineRequest>> GetPendingRequestsAsync();

        public Task<List<MedicineRequest>> GetRequestsByUserAsync(int userId);

        public Task<List<MedicineRequest>> GetRequestsByStatusAsync(RequestStatus status);

        public Task<MedicineRequest> AddRequestAsync(MedicineRequest request);

        public Task<MedicineRequest> UpdateRequestAsync(MedicineRequest request);

        public Task<MedicineRequest> UpdateRequestStatusAsync(int requestId, RequestStatus status, int? approvedByUserId = null);

        public Task<bool> DeleteRequestAsync(int id);
    }
}
