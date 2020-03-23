using Models.Enums;
using System;
using System.Collections.Generic;

namespace Models.Interfaces
{
    public interface IRideService
    {
        List<Ride> FindRides(Request request);

        Ride FindRide(int rideId);

        List<Booking> FindBookings(string userId);

        List<Ride> FindOfferedRides(string providerId);

        bool ApproveRequest(int rideId, string requestId, bool isApproved);

        void CreateRide(Ride ride);

        void RequestRide(string userId, int rideId,string pickUp,string drop,int noOfSeats);

        List<Request> GetRequests(int rideId);

        float CalculateCostForRide(int rideId, string pickUp, string drop);
    }
}
