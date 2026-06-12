using System.ComponentModel.DataAnnotations;

namespace EHRMvcAuditLedgerDemo.Validation
{
    public class FutureOrTodayDateAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext context)
        {
            if (value is DateTime date && date.Date < DateTime.Today)
                return new ValidationResult("Transaction date cannot be in the past");

            return ValidationResult.Success;
        }
    }
}

