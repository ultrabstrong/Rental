using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ApartmentWeb.Validation
{
    public class RequireIfEnumAttribute : ValidationAttribute, IClientValidatable
    {
        public string InstanceName { get; set; }

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

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
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
                ValidationType = nameof(RequireIfEnumAttribute).Replace(nameof(Attribute), "").ToLower()
            };
            rule.ValidationParameters[nameof(CheckIfName).ToLower()] = CheckIfName;
            rule.ValidationParameters[nameof(CheckIfValue).ToLower()] = CheckIfValue;

            yield return rule;
        }
    }
}
