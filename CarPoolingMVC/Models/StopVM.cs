using System.ComponentModel.DataAnnotations;

namespace CarPoolingMVC.Models
{
    public class StopVM
    {
        [Required]
        public string Location { get; set; }
    }
}
