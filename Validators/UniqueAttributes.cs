using MedicineStorage.Data;
using System.ComponentModel.DataAnnotations;

namespace MedicineStorage.Validators
{
    public class UniqueMedicineNameAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult("Name is required.");
            }

            var dbContext = (AppDbContext)validationContext.GetService(typeof(AppDbContext));
            if (dbContext == null)
            {
                throw new InvalidOperationException("Database context is not available.");
            }

            string name = value.ToString();
            bool exists = dbContext.Medicines.Any(m => m.Name == name);

            return exists ? new ValidationResult($"Medicine with the name '{name}' already exists.") : ValidationResult.Success;
        }
    }
}
