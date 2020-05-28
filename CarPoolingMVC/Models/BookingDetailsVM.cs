using System;
namespace CarPoolingMVC.Models
{
    public class BookingDetailsVM
    {
        public string Time { get; set; }

        public string RideStatus { get; set; }

        public byte[] ProviderPic { get; set; }

        public string RequestStatus { get; set; }

        public string From { get; set; }

        public string To { get; set; }

        public string ProviderName { get; set; }

        public float Cost { get; set; }

        public string VehicleNumber { get; set; }

        public VehicleTypeVM VehicleType { get; set; }

        public DateTime StartDate { get; set; }

        public int NoOfSeats { get; set; }
    }
}
