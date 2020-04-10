using System.ComponentModel.DataAnnotations;

namespace Models.ViewModels
{
    public class ViaPointVM
    {
        [Required]
        public string Location { get; set; }
    }
}
