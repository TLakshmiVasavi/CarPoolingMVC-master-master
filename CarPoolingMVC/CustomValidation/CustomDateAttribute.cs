using System;
using System.ComponentModel.DataAnnotations;

namespace CarPoolingMVC.CustomValidation
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CustomDateAttribute : ValidationAttribute
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
