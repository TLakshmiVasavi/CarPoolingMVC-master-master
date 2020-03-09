using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public RouteVM Route { get; set; }

        public VehicleVM Vehicle { get; set; }

        public DateTime StartDateTime { get; set; }
    }
}
