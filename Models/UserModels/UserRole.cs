using Microsoft.AspNetCore.Identity;

namespace MedicineStorage.Models.UserModels
{
    public class UserRole : IdentityUserRole<int>
    {
        public User User { get; set; } = null!;
        public AppRole Role { get; set; } = null!;
    }

    public class AppRole : IdentityRole<int>
    {
        public ICollection<UserRole> UserRoles { get; set; } = [];
    }
}
