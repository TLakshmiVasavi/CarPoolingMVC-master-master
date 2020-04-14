using System;

namespace CarPoolingMVC.Models
{
    public class AvailableRideVM
    {
        public int Id { get; set; }

        public VehicleVM Vehicle { get; set; }

        public DateTime StartTime { get; set; }

        public float Cost { get; set; }

        public string ProviderName { get; set; }

        public string ProviderId { get; set; }

        public RouteVM Route { get; set; }
    }
}
