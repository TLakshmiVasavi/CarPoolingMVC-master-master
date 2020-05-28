using System;
using System.ComponentModel;

namespace CarPoolingMVC.Models
{
    public class RequestVM
    {
        public DateTime StartDate { get; set; }

        public string Time { get; set; }

        public string From { get; set; }

        public string To { get; set; }

        //[DisplayName("Number Of Seats")]
        //public int NoOfSeats { get; set; }

        public VehicleTypeVM VehicleType { get; set; }
    }
}
