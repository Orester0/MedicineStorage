using MedicineStorage.Models.DTOs;
using MedicineStorage.Models.MedicineModels;
using MedicineStorage.Models.Params;
using MedicineStorage.Models.TenderModels;
using MedicineStorage.Models;
using MedicineStorage.Services.BusinessServices.Interfaces;
using MedicineStorage.Helpers;
using AutoMapper;
using MedicineStorage.Data.Interfaces;
using MimeKit;
using Humanizer;

namespace MedicineStorage.Services.BusinessServices.Implementations
{
    public class MedicineSupplyService(IMapper _mapper, IUnitOfWork _unitOfWork) : IMedicineSupplyService
    {
        public async Task<ServiceResult<PagedList<ReturnMedicineSupplyDTO>>> GetPaginatedSupplies(MedicineSupplyParams parameters)
        {
            var result = new ServiceResult<PagedList<ReturnMedicineSupplyDTO>>();
            var (supplies, totalCount) = await _unitOfWork.MedicineSupplyRepository.GetByParamsAsync(parameters);

            result.Data = new PagedList<ReturnMedicineSupplyDTO>(
                _mapper.Map<List<ReturnMedicineSupplyDTO>>(supplies),
                totalCount,
                parameters.PageNumber,
                parameters.PageSize
            );

            return result;
        }

        public async Task<ServiceResult<MedicineSupply>> CreateSupplyAsync(CreateMedicineSupplyDTO dto, int userId)
        {
            var result = new ServiceResult<MedicineSupply>();

            var medicine = await _unitOfWork.MedicineRepository.GetByIdAsync(dto.MedicineId);
            if (medicine == null)
            {
                throw new KeyNotFoundException($"Medicine with ID {dto.MedicineId} not found.");
            }

            var newSupply = new MedicineSupply
            {
                MedicineId = dto.MedicineId,
                Quantity = dto.Quantity,
                CreatedByUserId = userId,
                TransactionDate = DateTime.UtcNow
            };

            var createdSupply = await _unitOfWork.MedicineSupplyRepository.AddAsync(newSupply);
            medicine.Stock += newSupply.Quantity;
            _unitOfWork.MedicineRepository.Update(medicine);

            await _unitOfWork.CompleteAsync();
            result.Data = createdSupply;
            return result;
        }

        public async Task<ServiceResult<MedicineSupply>> CreateSupplyForTenderAsync(int medicineId, int quantity, int tenderId)
        {
            var result = new ServiceResult<MedicineSupply>();

            var medicine = await _unitOfWork.MedicineRepository.GetByIdAsync(medicineId);
            if (medicine == null)
            {
                throw new KeyNotFoundException($"Medicine with ID {medicineId} not found.");
            }

            var newSupply = new MedicineSupply
            {
                MedicineId = medicineId,
                Quantity = quantity,
                TenderId = tenderId,
                TransactionDate = DateTime.UtcNow
            };

            var createdSupply = await _unitOfWork.MedicineSupplyRepository.AddAsync(newSupply);
            medicine.Stock += newSupply.Quantity;
            _unitOfWork.MedicineRepository.Update(medicine);

            await _unitOfWork.CompleteAsync();
            result.Data = createdSupply;
            return result;
        }

    }
}
