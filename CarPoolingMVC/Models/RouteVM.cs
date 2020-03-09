using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CarPoolingMVC.Models
{
    public class RouteVM
    {
        [Required]
        public string Source { get; set; }

        [Required]
        public string Destination { get; set; }

        public List<ViaPointVM> ViaPoints { get; set; }

        [Required]
        public float TotalDistance { get; set; }
    }
}
