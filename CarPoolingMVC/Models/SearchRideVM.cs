using System.Collections.Generic;

namespace CarPoolingMVC.Models
{
    public class SearchRideVM
    {
        public RequestVM Request { get; set; }

        public List<AvailableRideVM> AvailableRides { get; set; }
    }
}
