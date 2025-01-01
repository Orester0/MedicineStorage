using MedicineStorage.Data.Interfaces;
using MedicineStorage.Models.AuditModels;
using MedicineStorage.Models.Tender;
using Microsoft.EntityFrameworkCore;

namespace MedicineStorage.Data.Implementations
{
    public class MedicineSupplyRepository(AppDbContext _context) : IMedicineSupplyRepository
    {
        public async Task<IEnumerable<MedicineSupply>> GetAllAsync()
        {
            return await _context.Set<MedicineSupply>()
                .Include(it => it.TenderProposalItem)
                .Include(it => it.Medicine)
                .ToListAsync();
        }

        public async Task<MedicineSupply> GetByIdAsync(int id)
        {
            return await _context.MedicineSupplies
                .Include(it => it.TenderProposalItem)
                .Include(it => it.Medicine)
                .FirstOrDefaultAsync(it => it.Id == id);
        }

        public async Task<MedicineSupply> CreateMedicineSupplyAsync(MedicineSupply medicineSupply)
        {
            await _context.MedicineSupplies.AddAsync(medicineSupply);
            return medicineSupply;
        }

        public void UpdateMedicineSupply(MedicineSupply medicineSupply)
        {
            _context.MedicineSupplies.Update(medicineSupply);
        }

        public async Task DeleteMedicineSupplyAsync(int supplyId)
        {
            var medicineSupply = await _context.MedicineSupplies.FindAsync(supplyId);
            if (medicineSupply != null)
            {
                _context.MedicineSupplies.Remove(medicineSupply);

            }
        }
    }
}