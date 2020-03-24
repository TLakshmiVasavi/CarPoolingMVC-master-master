using Models.Enums;
using System;
using System.Timers;

namespace Models
{
    public class Request
    {
        public string Id;

        public int RideId;

        public string RiderId;

        public string PickUp;

        public string Drop;

        public int NoOfSeats;

        public DateTime StartDateTime;

        public DateTime EndDateTime;

        public Timer timer;

        public VehicleType VehicleType;
    }
}
