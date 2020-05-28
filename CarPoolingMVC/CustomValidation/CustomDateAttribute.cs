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
            return DateTime.Parse(value.ToString()).Date >= DateTime.Now.Date;
        }
    }
    [AttributeUsage(AttributeTargets.Property)]
    public class CustomTimeAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return false;
            }
            string time = value.ToString();
            int hour = (time[1] == 'a' ? Convert.ToInt32(time[0]-'0') : Convert.ToInt32(time[0]-'0') + 12);
            return hour >= DateTime.Now.Hour;
        }
    }

}
