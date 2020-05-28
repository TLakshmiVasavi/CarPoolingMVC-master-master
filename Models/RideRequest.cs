using Models.Enums;
using System;
using System.Timers;

namespace Models
{
    public class RideRequest
    {
        public int Id;

        public int RideId;

        public string RiderId;

        public string From;

        public string To;

        public int NoOfSeats;

        public DateTime StartDate;

        public string Time;

        public Timer timer;

        public VehicleType VehicleType;
    }
}
