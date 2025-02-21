using MedicineStorage.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1;

namespace MedicineStorage.Data.Implementations
{
    public class GenericRepository<T>(AppDbContext _context) : IGenericRepository<T> where T : class
    {

        public virtual async Task<T?> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public virtual async Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            return entity;
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity == null) return false;
            _context.Set<T>().Remove(entity);
            return true;
        }
    }
}
