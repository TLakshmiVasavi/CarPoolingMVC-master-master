using System;
using System.ComponentModel.DataAnnotations;

namespace CarPooling.Models.ViewModels.CustomValidation
{
    [AttributeUsage(AttributeTargets.Property)]
    class CustomDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return false;
            }
            return DateTime.Parse(value.ToString()) >= DateTime.Now;
        }
    }
}
