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

        public DateTime StartDateTime;

        public DateTime EndDateTime;

        public int NoOfOfferedSeats;

        public float Distance;

        public float UnitDistanceCost;

        public List<Request> Requests;

        public List<Booking> Bookings;

        public Route Route;

        public bool IsRideCompleted => EndDateTime < DateTime.Now;

        public Ride()
        {
            Route = new Route();
        }
    }
}