using System.Collections.Generic;

namespace CarPoolingMVC.Models
{
    public class BookRideVM
    {
        public RequestVM Request { get; set; }

        public List<AvailableRideVM> AvailableRides { get; set; }
    }
}
