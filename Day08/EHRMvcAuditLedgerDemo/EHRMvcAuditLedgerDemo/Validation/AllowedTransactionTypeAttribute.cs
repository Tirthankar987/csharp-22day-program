// ValidationAttribute base class lives here — every custom validator inherits from it
// This is what makes [AllowedTransactionType] work exactly like [Required] or [Range]
using System.ComponentModel.DataAnnotations;

namespace EHRMvcAuditLedgerDemo.Validation
{
    // Naming convention: always end with "Attribute" — then C# lets you use it as [AllowedTransactionType]
    // dropping the "Attribute" suffix automatically. Both forms work:
    // [AllowedTransactionTypeAttribute] and [AllowedTransactionType] are identical
    public class AllowedTransactionTypeAttribute : ValidationAttribute
    {
        // ASP.NET Core calls this automatically during model binding — before your controller runs one line
        // You never call IsValid() yourself — the framework does it on every POST
        // Two parameters:
        //   value   = whatever the user actually typed into the TransactionType field
        //   context = metadata about the object being validated (class name, property name, DI services)
        protected override ValidationResult? IsValid(object? value, ValidationContext context)
        {
            // Hardcoded allowed values — fine for a demo
            // Real hospital: this list comes from a ReferenceData table in SQL
            // so adding "Insurance" as a new type is a DB insert, not a code change + redeployment
            // Hardcoding means every new transaction type requires a developer, a PR, and a release cycle
            var allowed = new[] { "Billing", "Pharmacy" };

            // Two failure cases handled in one line:
            // null   = user submitted nothing (empty field bypassed [Required] somehow)
            // not in allowed = user typed "Insurance", "Cash", or anything else not on the list
            // Real hospital: also add .Trim() before .Contains() — "Billing " with a trailing
            // space would fail this check and confuse everyone debugging it at 2am
            if (value == null || !allowed.Contains(value.ToString()))

                // Returns a failed ValidationResult with the error message
                // This message flows into ModelState automatically and surfaces in the Razor view
                // via <span asp-validation-for="TransactionType">
                // Real hospital: error messages are reviewed by compliance teams —
                // "must be Billing or Pharmacy" is fine for internal tools
                // but patient-facing systems use softer, non-technical language
                return new ValidationResult("Transaction type must be Billing or Pharmacy");

            // Returning Success tells the framework: this field passed, move on
            // Only after ALL attributes on ALL properties return Success does
            // ModelState.IsValid become true and your controller logic actually run
            return ValidationResult.Success;
        }
    }
}
