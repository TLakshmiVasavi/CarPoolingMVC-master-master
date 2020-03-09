using System;

namespace CarPoolingMVC.Models
{
    public class OfferedRideVM
    {
        public string Id { get; set; }

        public string VehicleId { get; set; }

        public VehicleTypeVM VehicleType { get; set; }

        public RouteVM Route { get; set; }

        public int NoOfOfferedSeats { get; set; }

        public bool IsRideCompleted { get; set; }

        public DateTime StartDateTime { get; set; }

    }
}
