using System;
using System.ComponentModel;

namespace CarPoolingMVC.Models
{
    public class RequestVM
    {
        public DateTime StartDateTime { get; set; }

        public string Source { get; set; }

        public string Destination { get; set; }

        [DisplayName("Number Of Seats")]
        public int NoOfSeats { get; set; }

        public VehicleTypeVM VehicleType { get; set; }
    }
}
