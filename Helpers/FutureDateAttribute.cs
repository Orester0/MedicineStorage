using System.ComponentModel.DataAnnotations;

namespace MedicineStorage.Helpers
{
    public class FutureDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return new ValidationResult("Date is required.");

            DateTime date = (DateTime)value;

            if (date.Date < DateTime.Now.Date)
            {
                return new ValidationResult("Date must be in the future.");
            }

            return ValidationResult.Success;
        }
    }

    public class FormerDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return new ValidationResult("Date is required.");

            DateTime date = (DateTime)value;

            if (date.Date > DateTime.Now.Date)
            {
                return new ValidationResult("Date cannot be in the future.");
            }

            return ValidationResult.Success;
        }
    }
}
