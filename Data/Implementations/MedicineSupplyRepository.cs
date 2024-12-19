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
                .ToListAsync();
        }

        public async Task<MedicineSupply> GetByIdAsync(int id)
        {
            return await _context.Set<MedicineSupply>()
                .Include(it => it.TenderProposalItem) 
                .FirstOrDefaultAsync(it => it.Id == id);
        }

        public async Task CreateMedicineSupplyAsync(MedicineSupply transaction)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            await _context.Set<MedicineSupply>().AddAsync(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMedicineSupplyAsync(MedicineSupply transaction)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            _context.Set<MedicineSupply>().Update(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMedicineSupplyAsync(int id)
        {
            var transaction = await GetByIdAsync(id);
            if (transaction == null) throw new KeyNotFoundException($"Transaction with ID {id} not found.");

            _context.Set<MedicineSupply>().Remove(transaction);
            await _context.SaveChangesAsync();
        }
    }
}