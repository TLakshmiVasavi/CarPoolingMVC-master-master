using Models.Enums;
using System;

namespace Models
{
    public class Ride
    {
        public int Id;

        public string VehicleId;

        public VehicleType VehicleType;

        public string ProviderId;

        public string ProviderName;

        public DateTime StartDate;

        public string Time;

        public int NoOfOfferedSeats;

        public int AvailableSeats;

        public float Distance;

        public float UnitDistanceCost;

        public Route Route;

        //public bool IsRideCompleted => StartDate < DateTime.Now.Date;

        public Ride()
        {
            Route = new Route();
        }
    }
}