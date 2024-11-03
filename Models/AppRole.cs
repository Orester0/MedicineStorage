using Microsoft.AspNetCore.Identity;

namespace MedicineStorage.Models
{
    public class AppRole : IdentityRole<int>
    {
        public ICollection<UserRole> UserRoles { get; set; } = [];
    }
}
