using CarPoolingMVC.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CarPoolingMVC.CustomValidation
{
    public class RequiredIfAttribute:ValidationAttribute
    {
        public string PropertyName { get; }
        public RequiredIfAttribute(string propertyName)
        {
            this.PropertyName = propertyName;
        }
        
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            object instance = context.ObjectInstance;
            Type type = instance.GetType();
            bool proprtyvalue = (bool)type.GetProperty(PropertyName).GetValue(instance);
            if (proprtyvalue)
            {
                var results = new List<ValidationResult>();
                Validator.TryValidateObject(value, new ValidationContext(value), results,true);
                if (results.Count != 0)
                {
                    //var compositeResults = new CompositeValidationResult(String.Format("Validation for {0} failed!", validationContext.DisplayName));
                    //results.ForEach(compositeResults.AddResult);

                    return new ValidationResult(ErrorMessage); 
                }
            }
            return ValidationResult.Success;
        }
    }
}
