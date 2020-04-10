using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models.ViewModels
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
