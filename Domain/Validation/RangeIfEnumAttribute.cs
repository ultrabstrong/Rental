using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Domain.Validation
{
    public class RangeIfEnumAttribute : ValidationAttribute, IClientValidatable
    {
        public string InstanceName { get; set; }

        private string CheckIfName { get; set; }

        private Type EnumType { get; set; }

        private int CheckIfValue { get; set; }

        private decimal? MinValue { get; set; } = null;

        private decimal? MaxValue { get; set; } = null;

        private short Accuracy { get; set; }

        public RangeIfEnumAttribute(string minval, short accuracy, string maxval, string checkIfName, object checkIfValue, string errorName, Type errorType)
        {
            MinValue = decimal.Round(Convert.ToDecimal(minval), accuracy);
            Accuracy = accuracy;
            MaxValue = decimal.Round(Convert.ToDecimal(maxval), accuracy);
            CheckIfName = checkIfName;
            EnumType = checkIfValue.GetType();
            CheckIfValue = (int)checkIfValue;
            ErrorMessageResourceName = errorName;
            ErrorMessageResourceType = errorType;
        }

        public RangeIfEnumAttribute(string minval, short accuracy, string checkIfName, object checkIfValue, string errorName, Type errorType)
        {
            MinValue = decimal.Round(Convert.ToDecimal(minval), accuracy);
            Accuracy = accuracy;
            MaxValue = null;
            CheckIfName = checkIfName;
            EnumType = checkIfValue.GetType();
            CheckIfValue = (int)checkIfValue;
            ErrorMessageResourceName = errorName;
            ErrorMessageResourceType = errorType;
        }

        public RangeIfEnumAttribute(short accuracy, string maxval, string checkIfName, object checkIfValue, string errorName, Type errorType)
        {
            MinValue = null;
            Accuracy = accuracy;
            MaxValue = decimal.Round(Convert.ToDecimal(maxval), accuracy);
            CheckIfName = checkIfName;
            EnumType = checkIfValue.GetType();
            CheckIfValue = (int)checkIfValue;
            ErrorMessageResourceName = errorName;
            ErrorMessageResourceType = errorType;
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var dependentValue = context.ObjectInstance.GetType().GetProperty(CheckIfName).GetValue(context.ObjectInstance, null);

            if (dependentValue != null && dependentValue.ToString() == ((Enum)Enum.ToObject(EnumType, CheckIfValue)).ToString())
            {
                decimal decimalvalue = Decimal.Round(Convert.ToDecimal(value), Accuracy);
                if (MinValue != null && decimalvalue < MinValue ||
                    MaxValue != null && decimalvalue > MaxValue)
                {
                    return new ValidationResult(FormatErrorMessage(context.DisplayName), new[] { context.MemberName });
                }
            }
            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ErrorMessageString,
                ValidationType = nameof(RangeIfEnumAttribute).Replace(nameof(Attribute), "").ToLower()
            };
            rule.ValidationParameters[nameof(MinValue).ToLower()] = MinValue;
            rule.ValidationParameters[nameof(MaxValue).ToLower()] = MaxValue;
            rule.ValidationParameters[nameof(Accuracy).ToLower()] = Accuracy;
            rule.ValidationParameters[nameof(CheckIfName).ToLower()] = CheckIfName;
            rule.ValidationParameters[nameof(CheckIfValue).ToLower()] = CheckIfValue;

            yield return rule;
        }
    }
}
