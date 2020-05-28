using Models.Enums;
using System;
using System.Collections.Generic;

namespace Models.Interfaces
{
    public interface IRideService
    {
        List<RideDetails> FindRides(RideRequest request);

        Ride FindRide(int rideId);

        List<Booking> FindBookings(string userId);

        List<Ride> FindOfferedRides(string providerId);

        bool ApproveRequest(int rideId, int requestId, bool isApproved);

        void CreateRide(Ride ride);

        void RequestRide(RideRequest request);

        List<RequestDetails> GetRequests(int rideId);

        RideRequest GetRequest(int requestId);

        float CalculateCostForRide(int rideId, string pickUp, string drop);
    }
}
