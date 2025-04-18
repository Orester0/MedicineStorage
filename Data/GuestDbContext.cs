using MedicineStorage.Models.UserModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MedicineStorage.Data
{
    public class GuestDbContext(DbContextOptions options) : AppDbContext(options)
    {

    }
}
