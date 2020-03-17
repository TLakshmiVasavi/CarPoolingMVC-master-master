using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Models.Interfaces;
using Models.Enums;

namespace Services
{
    public class RideService : IRideService
    {
        List<RideDetails> Details= new List<RideDetails>();
        List<Ride> Rides = new List<Ride>();
        double Time = 120000;
        
        public TimeSpan CalculateTime(float distance)
        {
            return TimeSpan.FromHours(distance / 50);
        }

        public TimeSpan CalculateTime(Ride ride, string pickUp, string drop)
        {
            return TimeSpan.FromHours(CalculateDistance(ride, pickUp, drop) / 50);
        }

        public void CreateRide(Ride ride)
        {
            ride.Bookings = new List<Booking>();
            ride.Requests = new List<Request>();
            ride.Id = ride.ProviderId.Substring(0, 4) + DateTime.Now.ToShortTimeString();
            ride.EndDateTime = ride.StartDateTime + CalculateTime(ride.Distance);
            RideDetails details = new RideDetails
            {
                ProviderId = ride.ProviderId,
                RideId = ride.Id,
                CoTraveller = new List<string>()
            };
            Details.Add(details);
            Rides.Add(ride);
        }

        public List<Ride> FindOfferedRides(string providerId)
        {
            return Rides.FindAll(r => r.ProviderId == providerId);
        }

        public Ride FindRide(string rideId)
        {
            return Rides.Find(r => r.Id == rideId);
        }

        public List<Ride> FindRides(string pickUp, string drop, DateTime dateTime,string riderId,int noOfSeats,VehicleType vehicleType)
        {
            return Rides.FindAll(r => r.ProviderId != riderId && r.VehicleType == vehicleType && IsEnRoute(r.Route, pickUp, drop) && IsSeatsAvailable(r, noOfSeats, pickUp, drop) && !IsRideStarted(pickUp, r) && r.StartDateTime.Date == dateTime);
        }

        public bool IsSeatsAvailable(Ride ride, int noOfSeats,string pickUp,string drop)
        {
            List<PlaceDetails> points = new List<PlaceDetails>();
            if (pickUp == ride.Route.Source)
            {
                points.Add(new PlaceDetails()
                {
                    count = 0,
                    distance = ride.Route[pickUp]
                });
            }
            foreach (var item in ride.Route.ViaPoints)
            {
                if (item.Distance < ride.Route[pickUp])
                {
                    continue;
                }
                if (item.Distance <= ride.Route[drop])
                {
                    points.Add(new PlaceDetails()
                    {
                        count = 0,
                        distance = ride.Route[pickUp]
                    });
                }
                else
                {
                    break;
                }
            }
            if (drop == ride.Route.Destination)
            {
                points.Add(new PlaceDetails()
                {
                    count = 0,
                    distance = ride.Route[drop]
                });
            }

            foreach (Booking booking in ride.Bookings.FindAll(_ => _.Response.Status == Status.Approved))
            {
                if (ride.Route[drop] > ride.Route[booking.Request.PickUp] && ride.Route[booking.Request.Drop] > ride.Route[pickUp])
                {
                    foreach (var item in points)
                    {
                        if (ride.Route[pickUp] <= item.distance && item.distance < ride.Route[drop])
                        {
                            item.count += booking.Request.NoOfSeats;
                            if (item.count+noOfSeats > ride.NoOfOfferedSeats)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        public bool IsRideStarted(string pickUp,Ride ride)
        {
            if (ride.Route.Source == pickUp && ride.StartDateTime<=DateTime.Now)
            {
                return true;
            }
            return ride.StartDateTime + CalculateTime(ride.Route[pickUp]) <= DateTime.Now;
        }

        public bool IsEnRoute(Route route, string pickUp, string drop)
        {
            if (route.Source == pickUp)
            {
                if(route.Destination == drop)
                {
                    return true;
                }
                if (route.ViaPoints.Any(v => v.Location == drop))
                {
                    return true;
                }
            }
            if (route.ViaPoints.Any(v => v.Location == pickUp))
            {
                if (route.Destination == drop)
                {
                    return true;
                }
                if (route.ViaPoints.Any(v => v.Location == drop))
                {
                    return true;
                }
            }
            return false;
        }

        public float CalculateDistance(Ride ride, string pickUp, string drop)
        {
            return ride.Route[drop] - ride.Route[pickUp];
        }

        public float CalculateCostForRide(string rideId,string pickUp,string drop)
        {
            Ride ride = FindRide(rideId);
            float dist = CalculateDistance(ride, pickUp, drop);
            return dist * ride.UnitDistanceCost;
        }

        public float CalculateCostForRide(string rideId, string requestId)
        {
            Request request = FindRide(rideId).Requests.Find(_ => _.RequestId == requestId);
            Ride ride = FindRide(rideId);
            float dist = CalculateDistance(ride, request.PickUp, request.Drop);
            return dist * ride.UnitDistanceCost;
        }

        public void RequestRide(string userId, string rideId, string pickUp, string drop, int noOfSeats)
        {
            Ride ride = FindRide(rideId);
            Request request = new Request
            {
                PickUp = pickUp,
                Drop = drop,
                RiderId = userId,
                NoOfSeats = noOfSeats,
                RequestId = userId + DateTime.Now
            };
            request.StartDateTime = ride.StartDateTime + CalculateTime(ride, ride.Route.Source, pickUp);
            request.EndDateTime = ride.StartDateTime + CalculateTime(ride, pickUp, drop);
            ride.Requests.Add(request);
            request.timer = new System.Timers.Timer
            {
                AutoReset = false,
                Enabled = true,
                Interval = Time
            };
            request.timer.Elapsed+=(sender,e)=>ApproveRequest(ride.Id,request.RequestId,false);
            request.timer.Start();
            Details.Find(r => r.RideId == ride.Id).CoTraveller.Add(userId);
        }

        public List<Ride> FindBookings(string userId)
        {
            return Rides.FindAll(r => IsRequested(userId, r.Id));
        }

        public bool IsRequested(string userId,string  rideId)
        {
            return Details.Find(r=>r.RideId==rideId).CoTraveller.Any(u=>u==userId);
        }
        
        public bool ApproveRequest(string rideId, string requestId, bool isApproved)
        {
            Ride ride = FindRide(rideId);
            Request request = ride.Requests.Find(_ => _.RequestId == requestId);
            request.timer.Stop();
            Booking booking = new Booking()
            {
                Response = new Response(),
                Request = request
            };
            ride.Requests.Remove(request);
            if (isApproved)
            {
                if (IsSeatsAvailable(ride, request.NoOfSeats, request.PickUp, request.Drop))
                {
                    booking.Response.Status = Status.Approved;
                }
                else
                {
                    booking.Response.Status = Status.Rejected;
                    ride.Bookings.Add(booking);
                    return false;
                }
            }
            else
            {
                booking.Response.Status = Status.Rejected;
            }
            ride.Bookings.Add(booking);
            return true;
        }

        public List<Request> GetRequests(string rideId)
        {
            Ride ride = FindRide(rideId);
            return ride.Requests;
        }
    }
}