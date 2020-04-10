using System.Collections.Generic;

namespace Models.ViewModels
{
    public class SearchRideVM
    {
        public RequestVM Request { get; set; }

        public List<AvailableRideVM> AvailableRides { get; set; }
    }
}
