using Models.Enums;
using System;
using System.Collections.Generic;

namespace Models
{
    public class Ride
    {
        public int Id;

        public string VehicleId;

        public VehicleType VehicleType;

        public string ProviderId;

        public string ProviderName;

        //public DateTime StartDateTime;

        //public DateTime EndDateTime;
        public DateTime StartDate;

        public string Time;

        public int NoOfOfferedSeats;

        public float Distance;

        public float UnitDistanceCost;

        public List<RideRequest> Requests;

        public List<Booking> Bookings;

        public Route Route;

        //public bool IsRideCompleted => StartDate < DateTime.Now.Date;

        public Ride()
        {
            Route = new Route();
        }
    }
}