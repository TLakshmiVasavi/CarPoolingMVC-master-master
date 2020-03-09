﻿using System;
using System.Linq;
using System.Threading.Tasks;

namespace CarPoolingMVC.Models
{
    public class AvailableRideVM
    {
        public string Id { get; set; }

        public VehicleVM Vehicle { get; set; }

        public DateTime StartTime { get; set; }

        public float Cost { get; set; }

        public string ProviderName { get; set; }

        public string ProviderId { get; set; }

        public RouteVM Route { get; set; }
    }
}
