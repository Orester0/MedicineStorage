using MedicineStorage.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MedicineStorage.Services.Interfaces
{
    public interface IUserRepository
    {
        public Task AddAsync(User entity);

        public Task DeleteAsync(User entity);

        public Task<IEnumerable<User>> GetAllAsync();

        public Task<IEnumerable<User>> GetByConditionAsync(Expression<Func<User, bool>> expression);
        public Task<User> GetByIdAsync(int id);

        public Task<User?> GetByUserNameAsync(string UserName);

        public void Update(User entity);
    }
}
