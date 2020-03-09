using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarPoolingMVC.Models
{
    public class SearchRideVM
    {
        public RequestVM Request { get; set; }

        public List<AvailableRideVM> AvailableRides { get; set; }
    }
}
