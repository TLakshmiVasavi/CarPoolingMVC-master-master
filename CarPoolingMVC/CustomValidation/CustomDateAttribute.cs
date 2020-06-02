using System;
using System.ComponentModel.DataAnnotations;

namespace CarPoolingMVC.CustomValidation
{
    public class CustomTimeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            object instance = context.ObjectInstance;
            Type type = instance.GetType();
            DateTime date = (DateTime)type.GetProperty("StartDate").GetValue(instance);
            if (DateTime.Parse(date.ToString()).Date > DateTime.Now.Date)
            {
                return ValidationResult.Success;
            }
            else
            if(DateTime.Parse(date.ToString()).Date == DateTime.Now.Date)
            { 
                if (value == null)
                {
                    return new ValidationResult(ErrorMessage);
                }
                string time = value.ToString();
                int hour = (time[1] == 'a' ? Convert.ToInt32(time[0] - '0') : Convert.ToInt32(time[0] - '0') + 12);
                return hour >= DateTime.Now.Hour?ValidationResult.Success: new ValidationResult(ErrorMessage);

            }
            else
            {
                return new ValidationResult(ErrorMessage);

            }
            //if (proprtyvalue)
            //{
            //var results = new List<ValidationResult>();
            //Validator.TryValidateObject(value, new ValidationContext(value), results, true);
            //if (results.Count != 0)
            //{
            //    //var compositeResults = new CompositeValidationResult(String.Format("Validation for {0} failed!", validationContext.DisplayName));
            //    //results.ForEach(compositeResults.AddResult);

            //    return new ValidationResult(ErrorMessage);
            //}
            //}
            
        }
    }
    //[AttributeUsage(AttributeTargets.Property)]
    //public class CustomDateAttribute : ValidationAttribute
    //{
    //    public override bool IsValid(object value,ValidationContext validationContext)
    //    {
    //        if (value == null)
    //        {
    //            return false;
    //        }
    //        return DateTime.Parse(value.ToString()).Date >= DateTime.Now.Date;
    //    }
    //}
    // [AttributeUsage(AttributeTargets.Property)]
    //public class CustomTimeAttribute : ValidationAttribute
    //{
    //    public override bool IsValid(object value,ValidationContext validationContext)
    //    {
//            if (value == null)
//            {
//                return false;
//            }
//string time = value.ToString();
//int hour = (time[1] == 'a' ? Convert.ToInt32(time[0] - '0') : Convert.ToInt32(time[0] - '0') + 12);
//            return hour >= DateTime.Now.Hour;
    //    }
    //}

}
