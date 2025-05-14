using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ApartmentWeb.Validation
{
    public class RequireIfEnumEnabledAttribute : ValidationAttribute, IClientValidatable
    {
        public string InstanceName { get; set; }

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

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            if (!GetIsCheckEnabled(context.ObjectInstance))
            {
                return ValidationResult.Success;
            }

            var dependentValue = context.ObjectInstance.GetType().GetProperty(CheckIfName).GetValue(context.ObjectInstance, null);

            if (dependentValue != null && dependentValue.ToString() == ((Enum)Enum.ToObject(EnumType, CheckIfValue)).ToString())
            {
                if (!_innerAttribute.IsValid(value))
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
                ErrorMessage = ErrorMessage ?? ErrorMessageString,
                ValidationType = nameof(RequireIfEnumEnabledAttribute).Replace(nameof(Attribute), "").ToLower()
            };
            rule.ValidationParameters[nameof(CheckIfName).ToLower()] = CheckIfName;
            rule.ValidationParameters[nameof(CheckIfValue).ToLower()] = CheckIfValue;
            rule.ValidationParameters[nameof(IsCheckEnabled).ToLower()] = GetIsCheckEnabled(metadata.Container);

            yield return rule;
        }

        private bool GetIsCheckEnabled(object instance)
        {
            var enabledProperty = instance.GetType().GetProperty(IsCheckEnabled);
            if (enabledProperty == null || !(enabledProperty.GetValue(instance) is bool isEnabled))
            {
                return false;
            }
            return isEnabled;
        }
    }
}