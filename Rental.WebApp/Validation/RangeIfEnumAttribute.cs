using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Rental.WebApp.Validation;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
internal sealed class RangeIfEnumAttribute : ValidationAttribute, IClientModelValidator
{
    private string CheckIfName { get; set; }

    private Type EnumType { get; set; }

    private int CheckIfValue { get; set; }

    private decimal? MinValue { get; set; } = null;

    private decimal? MaxValue { get; set; } = null;

    private short Accuracy { get; set; }

    public RangeIfEnumAttribute(string minval, short accuracy, string maxval, string checkIfName, object checkIfValue, string errorMessage)
        : this(accuracy, checkIfName, checkIfValue, errorMessage)
    {
        MaxValue = decimal.Round(Convert.ToDecimal(maxval), accuracy);
        MinValue = decimal.Round(Convert.ToDecimal(minval), accuracy);
    }

    public RangeIfEnumAttribute(string minval, short accuracy, string checkIfName, object checkIfValue, string errorMessage)
        : this(accuracy, checkIfName, checkIfValue, errorMessage)
    {
        MinValue = decimal.Round(Convert.ToDecimal(minval), accuracy);
    }

    public RangeIfEnumAttribute(short accuracy, string maxval, string checkIfName, object checkIfValue, string errorMessage)
        : this(accuracy, checkIfName, checkIfValue, errorMessage)
    {
        MaxValue = decimal.Round(Convert.ToDecimal(maxval), accuracy);
    }

    private RangeIfEnumAttribute(short accuracy, string checkIfName, object checkIfValue, string errorMessage)
    {
        Accuracy = accuracy;
        CheckIfName = checkIfName;
        EnumType = checkIfValue.GetType();
        CheckIfValue = (int)checkIfValue;
        ErrorMessage = errorMessage;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext context)
    {
        ArgumentNullException.ThrowIfNull(context?.MemberName);

        var dependentValue = context.ObjectInstance.GetType().GetProperty(CheckIfName)?.GetValue(context.ObjectInstance, null);

        if (dependentValue != null && dependentValue.ToString() == ((Enum)Enum.ToObject(EnumType, CheckIfValue)).ToString())
        {
            decimal decimalvalue = Decimal.Round(Convert.ToDecimal(value), Accuracy);
            if (MinValue != null && decimalvalue < MinValue ||
                MaxValue != null && decimalvalue > MaxValue)
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
        MergeAttribute(context.Attributes, "data-val-rangeifenum", FormatErrorMessage(context.ModelMetadata.GetDisplayName()));

        if (MinValue.HasValue)
        {
            MergeAttribute(context.Attributes, "data-val-rangeifenum-minvalue", MinValue.Value.ToString());
        }

        if (MaxValue.HasValue)
        {
            MergeAttribute(context.Attributes, "data-val-rangeifenum-maxvalue", MaxValue.Value.ToString());
        }

        MergeAttribute(context.Attributes, "data-val-rangeifenum-accuracy", Accuracy.ToString());
        MergeAttribute(context.Attributes, "data-val-rangeifenum-checkifname", CheckIfName);
        MergeAttribute(context.Attributes, "data-val-rangeifenum-checkifvalue", CheckIfValue.ToString());
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
