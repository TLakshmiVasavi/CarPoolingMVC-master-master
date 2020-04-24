using Models.Enums;
using System;
using System.Timers;

namespace Models
{
    public class RideRequest
    {
        public string Id;

        public int RideId;

        public string RiderId;

        public string PickUp;

        public string Drop;

        public int NoOfSeats;

        public DateTime StartDate;

        public string Time;

        public Timer timer;

        public VehicleType VehicleType;
    }
}
