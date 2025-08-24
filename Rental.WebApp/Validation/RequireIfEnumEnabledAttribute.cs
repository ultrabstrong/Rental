using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Rental.WebApp.Validation;

public class RequireIfEnumEnabledAttribute : ValidationAttribute, IClientModelValidator
{
    private string CheckIfName { get; set; }

    private Type EnumType { get; set; }

    private int CheckIfValue { get; set; }

    private string IsCheckEnabled { get; set; }

    private readonly RequiredAttribute _innerAttribute;

    public RequireIfEnumEnabledAttribute(string checkIfName, object checkIfValue, string isCheckEnabled, string errorMessage)
        : this(checkIfName, checkIfValue, isCheckEnabled)
    {
        ErrorMessage = errorMessage;
    }

    public RequireIfEnumEnabledAttribute(string checkIfName, object checkIfValue, string enabledPropertyName)
    {
        CheckIfName = checkIfName;
        EnumType = checkIfValue.GetType();
        CheckIfValue = (int)checkIfValue;
        IsCheckEnabled = enabledPropertyName;
        _innerAttribute = new RequiredAttribute();
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext context)
    {
        ArgumentNullException.ThrowIfNull(context?.MemberName);

        if (!GetIsCheckEnabled(context.ObjectInstance))
        {
            return ValidationResult.Success;
        }

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
        MergeAttribute(context.Attributes, "data-val-requireifenumenabled", FormatErrorMessage(context.ModelMetadata.GetDisplayName()));
        MergeAttribute(context.Attributes, "data-val-requireifenumenabled-checkifname", CheckIfName);
        MergeAttribute(context.Attributes, "data-val-requireifenumenabled-checkifvalue", CheckIfValue.ToString());
        MergeAttribute(context.Attributes, "data-val-requireifenumenabled-ischeckenabled", IsCheckEnabled);
    }

    private static void MergeAttribute(IDictionary<string, string> attributes, string key, string value)
    {
        if (attributes.ContainsKey(key))
        {
            return;
        }
        attributes.Add(key, value);
    }

    private bool GetIsCheckEnabled(object instance)
    {
        var enabledProperty = instance.GetType().GetProperty(IsCheckEnabled);
        if (enabledProperty == null || enabledProperty.GetValue(instance) is not bool isEnabled)
        {
            return false;
        }
        return isEnabled;
    }
}