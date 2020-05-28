using System;

namespace CarPoolingMVC.Models
{
    public class OfferedRideVM
    {
        public int Id { get; set; }

        public string VehicleId { get; set; }

        //public VehicleTypeVM VehicleType { get; set; }

        //public RouteVM Route { get; set; }
        public string From { get; set; }

        public string To { get; set; }

        public int NoOfOfferedSeats { get; set; }

        public string Status { get; set; }

        public DateTime StartDate { get; set; }

        public string Time { get; set; }

        public float Cost { get; set; }

    }
}
