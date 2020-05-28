using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Models.Interfaces;
using Models.Enums;
using Models.DAL;
using Models.DAL.AppConfig;

namespace Services
{
    public class RideService : IRideService
    {
        RideDal RideDal = new RideDal(new AppConfiguration());
        BookingDal BookingDal = new BookingDal(new AppConfiguration());
        
        public void CreateRide(Ride ride)
        {
            RideDal.Create(ride);
        }

        public List<Ride> FindOfferedRides(string providerId)
        {
            return RideDal.FindOfferedRides(providerId);
        }

        public Ride FindRide(int rideId)
        {
            return RideDal.GetById(rideId);
        }

        public List<RideDetails> FindRides(RideRequest request)
        {
            return RideDal.BookRide(request);
        }

        public float CalculateCostForRide(int rideId,string pickUp,string drop)
        {
            return RideDal.GetCost(rideId, pickUp, drop);
        }

        public void RequestRide(RideRequest request)
        {
            BookingDal.Create(request);
        }

        public List<Booking> FindBookings(string userId)
        {
            return BookingDal.GetBookings(userId);
        }

        public bool IsRequested(string userId,int rideId)
        {
            return RideDal.IsBooked(rideId, userId);
        }
        
        public bool ApproveRequest(int rideId, int requestId, bool isApproved)
        {
            if (isApproved)
            {
                if (BookingDal.GetStatus(requestId).ToString() == "Requested")
                {
                    RideRequest request = BookingDal.GetBookingDetails(requestId).Request;
                    if (RideDal.IsSeatAvailable(request, rideId))
                    {
                        BookingDal.UpdateStatus(requestId, "Approved");
                        return true;
                    }
                    else
                    {
                        BookingDal.UpdateStatus(requestId, "Rejected");
                        return false;
                    }
                }
            }
            else
            {
                BookingDal.UpdateStatus(requestId, "Rejected");
            }
            return true;

        }

        public List<RequestDetails> GetRequests(int rideId)
        {
            return BookingDal.GetRequests(rideId);
        }

        public RideRequest GetRequest(int requestId)
        {
            return BookingDal.GetRequest(requestId);
        }

    }
}