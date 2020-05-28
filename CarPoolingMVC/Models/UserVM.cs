using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CarPoolingMVC.Models
{
    public class UserVM
    {
        public string Id { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please enter name"), MaxLength(30)]
        public string Name { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Please enter Email ID")]
        [RegularExpression(@"^\w+\@\w+\.[a-zA-z]{2,3}$", ErrorMessage = "Please enter a valid email.")]
        [Remote(action: "IsUserExists", controller: "User", ErrorMessage = "The Mail Already Exists")]
        public string Mail { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Please enter Password")]
        [RegularExpression(@"^((?=.*\d)(?=.*[A-Z])(?=.*[^A-Za-z0-9])).{6,}", ErrorMessage = "Please choose a Strong Password.")]
        public string Password { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "Please enter Phone Number")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Please enter the phone number.")]
        public string Number { get; set; }

        [Required(ErrorMessage = "Please enter Age")]
        [Range(1, 100, ErrorMessage = "Please enter a valid Value")]
        public int Age { get; set; }

        [DefaultValue(true)]
        public bool HasVehicle { get; set; }

        public GenderVM Gender { get; set; }

        public VehicleVM Vehicle { get; set; }

        public IFormFile Photo { get; set; }

    }
}
