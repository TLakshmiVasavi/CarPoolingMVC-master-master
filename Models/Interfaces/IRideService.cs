using Models.Enums;
using System;
using System.Collections.Generic;

namespace Models.Interfaces
{
    public interface IRideService
    {
        List<Ride> FindRides(string pickup, string drop, DateTime dateTime,string riderId,int noOfSeats,VehicleType vehicleType);

        Ride FindRide(string rideId);

        List<Ride> FindBookings(string userId);

        List<Ride> FindOfferedRides(string providerId);

        float CalculateDistance(Ride ride, string pickUp, string drop);

        bool ApproveRequest(string rideId, string requestId, bool isApproved);

        void CreateRide(Ride ride);

        void RequestRide(string userId, string rideId,string pickUp,string drop,int noOfSeats);

        TimeSpan CalculateTime(float distance);

        TimeSpan CalculateTime(Ride ride, string pickUp, string drop);

        float CalculateCostForRide(string rideId, string pickUp, string drop);

        float CalculateCostForRide(string rideId, string requestId);

        bool IsRideStarted(string pickUp, Ride ride);

        bool IsEnRoute(Route route, string pickUp, string drop);

        bool IsSeatsAvailable(Ride ride,int noOfSeats,string pickUp,string drop);

        bool IsRequested(string userId, string rideId);

        List<Request> GetRequests(string rideId);
    }
}
