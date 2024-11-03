using MedicineStorage.Models;
using MedicineStorage.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MedicineStorage.Data
{
    public class UserRepository(AppDbContext _context) : IUserRepository
    {
        public Task AddAsync(User entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(User entity)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public Task<IEnumerable<User>> GetByConditionAsync(Expression<Func<User, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }
        public async Task<User?> GetByUserNameAsync(string UserName)
        {
            return await _context.Users.SingleOrDefaultAsync(x => x.NormalizedUserName == UserName.ToUpper());
        }

        public void Update(User entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }
        
    }
}
