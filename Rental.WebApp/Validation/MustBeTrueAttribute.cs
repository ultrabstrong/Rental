using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Rental.WebApp.Validation;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public sealed class MustBeTrueAttribute : ValidationAttribute, IClientModelValidator
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is bool b && b)
            return ValidationResult.Success;
        return new ValidationResult(ErrorMessage ?? $"{validationContext.DisplayName} must be accepted");
    }

    public void AddValidation(ClientModelValidationContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        if (!context.Attributes.ContainsKey("data-val"))
        {
            context.Attributes.Add("data-val", "true");
        }
        context.Attributes["data-val-mustbetrue"] = FormatErrorMessage(context.ModelMetadata.GetDisplayName());
    }
}
