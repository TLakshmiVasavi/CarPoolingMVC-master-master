using System.ComponentModel;

namespace Models.ViewModels
{
    public class VehicleVM
    {
        [DisplayName("Vehicle Number")]
        public string Number { get; set; }

        public string Model { get; set; }

        public int? Capacity { get; set; }

        public VehicleTypeVM Type { get; set; }
    }
}
