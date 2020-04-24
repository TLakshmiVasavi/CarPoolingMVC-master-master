using System;
namespace CarPoolingMVC.Models
{
    public class BookingDetailsVM
    {
        public bool IsRideCompleted { get; set; }

        public string Status { get; set; }

        public string PickUp { get; set; }

        public string Drop { get; set; }

        public string ProviderName { get; set; }

        public float Cost { get; set; }

        public string VehicleNumber { get; set; }

        public VehicleTypeVM VehicleType { get; set; }

        public DateTime StartDate { get; set; }

        public int NoOfSeats { get; set; }
    }
}
