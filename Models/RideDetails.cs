
using System;

namespace Models
{
    public class RideDetails
    {
        public int Id { get; set; }

        public Vehicle Vehicle { get; set; }

        public DateTime StartDate { get; set; }

        public string Time { get; set; }

        public float Cost { get; set; }

        public string ProviderName { get; set; }

        public byte[] ProviderPic { get; set; }

        public string ProviderId { get; set; }

        public int AvailableSeats { get; set; }
    }
}
