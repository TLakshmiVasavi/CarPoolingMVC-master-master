using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CarPoolingMVC.Models
{
    public class RouteVM
    {
        [Required]
        public string From { get; set; }

        [Required]
        public string To { get; set; }

        public List<string> Stops { get; set; }

        [Required]
        public float TotalDistance { get; set; }
    }
}
