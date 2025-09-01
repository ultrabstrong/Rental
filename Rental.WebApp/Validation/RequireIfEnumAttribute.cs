using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Rental.WebApp.Validation;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
internal sealed class RequireIfEnumAttribute : ValidationAttribute, IClientModelValidator
{
    private string CheckIfName { get; set; }

    private Type EnumType { get; set; }

    private int CheckIfValue { get; set; }

    private readonly RequiredAttribute _innerAttribute;

    public RequireIfEnumAttribute(string checkIfName, object checkIfValue, string errorMessage)
        : this(checkIfName, checkIfValue)
    {
        ErrorMessage = errorMessage;
    }

    public RequireIfEnumAttribute(string checkIfName, object checkIfValue)
    {
        CheckIfName = checkIfName;
        EnumType = checkIfValue.GetType();
        CheckIfValue = (int)checkIfValue;
        _innerAttribute = new RequiredAttribute();
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext context)
    {
        ArgumentNullException.ThrowIfNull(context?.MemberName);

        var dependentValue = context.ObjectInstance.GetType().GetProperty(CheckIfName)?.GetValue(context.ObjectInstance, null);

        if (dependentValue != null && dependentValue.ToString() == ((Enum)Enum.ToObject(EnumType, CheckIfValue)).ToString())
        {
            if (!_innerAttribute.IsValid(value))
            {
                return new ValidationResult(FormatErrorMessage(context.DisplayName), [context.MemberName]);
            }
        }
        return ValidationResult.Success;
    }

    public void AddValidation(ClientModelValidationContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        MergeAttribute(context.Attributes, "data-val", "true");
        MergeAttribute(context.Attributes, "data-val-requireifenum", FormatErrorMessage(context.ModelMetadata.GetDisplayName()));
        MergeAttribute(context.Attributes, "data-val-requireifenum-checkifname", CheckIfName);
        MergeAttribute(context.Attributes, "data-val-requireifenum-checkifvalue", CheckIfValue.ToString());
    }

    private static void MergeAttribute(IDictionary<string, string> attributes, string key, string value)
    {
        if (attributes.ContainsKey(key))
        {
            return;
        }
        attributes.Add(key, value);
    }
}
