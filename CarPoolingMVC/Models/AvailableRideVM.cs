using System;

namespace CarPoolingMVC.Models
{
    public class AvailableRideVM
    {
        public int Id { get; set; }

        public VehicleVM Vehicle { get; set; }

        public DateTime StartDate { get; set; }

        public string Time { get; set; }

        public float Cost { get; set; }

        public string ProviderName { get; set; }

        public byte[] ProviderPic { get; set; }

        public string ProviderId { get; set; }

        public  string From { get; set; }

        public string To { get; set; }

        public int AvailableSeats { get; set; }
    }
}
