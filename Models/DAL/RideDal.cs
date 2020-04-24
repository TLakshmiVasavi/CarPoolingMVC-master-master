using Models.DAL.AppConfig;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;

namespace Models.DAL
{
    public class RideDal
    {
        private readonly AppConfiguration Configuration;
        VehicleDal VehicleDal = new VehicleDal(new AppConfiguration());
        UserDal UserDal = new UserDal(new AppConfiguration());

        public RideDal(AppConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void Create(Ride ride)
        {
            string connectionString = Configuration.ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string locations= ride.Route.Source+","+string.Join(",",ride.Route.ViaPoints.Select(_ => _.Location).ToList())+ ((ride.Route.ViaPoints.Count > 0) ? "," : "") + ride.Route.Destination;
                string distances = string.Join(",", ride.Route.ViaPoints.Select(_ => _.Distance).ToList()) + ((ride.Route.ViaPoints.Count>0)?",":"") + ride.Distance;
//                string durations = string.Join(",", ride.Route.ViaPoints.Select(_ => _.Duration).ToList()) + ((ride.Route.ViaPoints.Count > 0) ? "," : "") + (ride.EndDateTime - ride.StartDate);
                string durations = string.Join(",", ride.Route.ViaPoints.Select(_ => _.Duration).ToList()) + ((ride.Route.ViaPoints.Count > 0) ? "," : "") + (ride.StartDate);
                string sql = $"Insert Into Ride (VehicleId, ProviderId, NoOfOfferedSeats, UnitDistanceCost, StartDate,Time, Locations," +
                    $" Distances, Durations,Duration,Distance) Values ('{ride.VehicleId}','{ride.ProviderId}'," +
                    $"'{ride.NoOfOfferedSeats}','{ride.UnitDistanceCost}','{ride.StartDate}','{locations}'," +
                    $"'{distances}','{durations}','{ride.StartDate}','{ride.Distance}')";
                    //string sql = $"Insert Into Ride (VehicleId, ProviderId, NoOfOfferedSeats, UnitDistanceCost, StartDate,Time, Locations," +
                    //$" Distances, Durations,Duration,Distance) Values ('{ride.VehicleId}','{ride.ProviderId}'," +
                    //$"'{ride.NoOfOfferedSeats}','{ride.UnitDistanceCost}','{ride.StartDate}','{locations}'," +
                    //$"'{distances}','{durations}','{ride.EndDateTime - ride.StartDate}','{ride.Distance}')";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public void Update(Ride ride)//need to change
        {
            string connectionString = Configuration.ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string locations = ride.Route.Source + "," + string.Join(",", ride.Route.ViaPoints.Select(_ => _.Location).ToList()) + ((ride.Route.ViaPoints.Count > 0) ? "," : "") + ride.Route.Destination;
                string distances = string.Join(",", ride.Route.ViaPoints.Select(_ => _.Distance).ToList()) + ((ride.Route.ViaPoints.Count > 0) ? "," : "") + ride.Distance;
//                string durations = string.Join(",", ride.Route.ViaPoints.Select(_ => _.Duration).ToList()) + ((ride.Route.ViaPoints.Count > 0) ? "," : "") + (ride.EndDateTime - ride.StartDate);
                string durations = string.Join(",", ride.Route.ViaPoints.Select(_ => _.Duration).ToList()) + ((ride.Route.ViaPoints.Count > 0) ? "," : "") + (ride.StartDate);
                string sql = $"Update Ride set VehicleId='{ride.VehicleId}', ProviderId='{ride.ProviderId}', NoOfOfferedSeats='{ride.NoOfOfferedSeats}', UnitDistanceCost='{ride.UnitDistanceCost}', StartDate='{ride.StartDate}', Locations='{locations}', Distances='{distances}' where id='{ride.Id}'";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public List<Ride> GetAllRides()
        {
            List<Ride> rides = new List<Ride>();
            string connectionString = Configuration.ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = $"Select * From Ride ";
                SqlCommand command = new SqlCommand(sql, connection);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        List<string> locations;
                        Ride Ride = new Ride
                        {
                            Id = Convert.ToInt32(dataReader["Id"]),
                            NoOfOfferedSeats = Convert.ToInt32(dataReader["NoOfOfferedSeats"]),
                            StartDate = Convert.ToDateTime(dataReader["StartDate"]),
                            UnitDistanceCost = float.Parse(Convert.ToString(dataReader["UnitDistanceCost"])),
                            VehicleId = Convert.ToString(dataReader["VehicleId"]),
                        };
                        locations = Convert.ToString(dataReader["locations"]).Split(",").ToList();
                        Ride.VehicleType = VehicleDal.GetVehicleType(Ride.VehicleId);
                        Ride.ProviderName = UserDal.GetUserName(Ride.ProviderId);
                        Ride.Route = new Route()
                        {
                            Source = locations.First(),
                            Destination = locations.Last(),
                            ViaPoints = new List<ViaPoint>()
                        };
                        locations.RemoveAt(0);
                        locations.RemoveAt(locations.Count - 1);
                        locations.ForEach(_ => Ride.Route.ViaPoints.Add(new ViaPoint() { Location = _ }));
                        rides.Add(Ride);
                    }
                }
                connection.Close();
            }
            return rides;
        }

        public Ride GetById(int rideId)
        {
            Ride Ride = new Ride();
            string connectionString = Configuration.ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = $"Select * From Ride where RideId='{rideId}' ";
                SqlCommand command = new SqlCommand(sql, connection);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        Ride.Id = Convert.ToInt32(dataReader["Id"]);
                        Ride.NoOfOfferedSeats = Convert.ToInt32(dataReader["NoOfOfferedSeats"]);
                        Ride.StartDate = Convert.ToDateTime(dataReader["StartDate"]);
                        Ride.UnitDistanceCost = float.Parse(Convert.ToString(dataReader["UnitDistanceCost"]));
                        Ride.VehicleId = Convert.ToString(dataReader["VehicleId"]);
                    }
                }
                connection.Close();
            }
            return Ride;
        }

        public List<Ride> FindOfferedRides(string providerId)
        {
            List<Ride> rides = new List<Ride>();
            string connectionString = Configuration.ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = $"Select * From Ride where ProviderId='{providerId}' ";
                SqlCommand command = new SqlCommand(sql, connection);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        List<string> locations;
                        Ride Ride = new Ride
                        {
                            Id = Convert.ToInt32(dataReader["Id"]),
                            NoOfOfferedSeats = Convert.ToInt32(dataReader["NoOfOfferedSeats"]),
                            StartDate = Convert.ToDateTime(dataReader["StartDate"]),
                            UnitDistanceCost = float.Parse(Convert.ToString(dataReader["UnitDistanceCost"])),
                            VehicleId = Convert.ToString(dataReader["VehicleId"]),
                            Time=Convert.ToString(dataReader["Time"])
                        };
                        locations = Convert.ToString(dataReader["locations"]).Split(",").ToList();
                        Ride.VehicleType = VehicleDal.GetVehicleType(Ride.VehicleId);
                        Ride.ProviderName = UserDal.GetUserName(Ride.ProviderId);
                        Ride.Route = new Route()
                        {
                            Source = locations.First(),
                            Destination = locations.Last(),
                            ViaPoints = new List<ViaPoint>()
                        };
                        locations.RemoveAt(0);
                        locations.RemoveAt(locations.Count - 1);
                        locations.ForEach(_ => Ride.Route.ViaPoints.Add(new ViaPoint() { Location = _ }));
                        rides.Add(Ride);
                    }
                }
                connection.Close();
            }
            return rides;
        }

        public bool IsSeatAvailable(RideRequest request,int rideId)
        {
            List<string> locations=new List<string>();
            List<float> distances=new List<float>();
            List<PlaceDetails> placesDetails = new List<PlaceDetails>();
            string connectionString = Configuration.ConnectionString;
            int NoOfOfferedSeats=0;
            bool flag = true;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = $"select locations,distances,NoOfOfferedSeats from Ride where Id='{rideId}'";
                SqlCommand command = new SqlCommand(sql, connection);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        NoOfOfferedSeats = Convert.ToInt32(dataReader["NoOfOfferedSeats"]);
                        locations = Convert.ToString(dataReader["Locations"]).Split(",").ToList();
                        distances = Convert.ToString(dataReader["Distances"]).Split(",").ToList().ConvertAll(float.Parse);
                        distances.ForEach(x => placesDetails.Add(new PlaceDetails() { distance = x, count = 0 }));
                    }
                }
                sql = $"select * from Booking where RideId='{rideId}' and status='Approved'";
                command = new SqlCommand(sql, connection);
                using (SqlDataReader dataReader = command.ExecuteReader())
                { 
                    while (dataReader.Read())
                    {
                        int noOfSeats = Convert.ToInt32(dataReader["NoOfOfferedSeats"]);
                        float pickUpdist=distances[locations.FindIndex(x => x == request.PickUp)];
                        float dropdist=distances[locations.FindIndex(x => x == request.Drop)];
                        foreach (var item in placesDetails)
                        {
                            if (pickUpdist <= item.distance && item.distance < dropdist)
                            {
                                item.count += request.NoOfSeats;
                                if (item.count + noOfSeats > NoOfOfferedSeats)
                                {
                                    flag=false;
                                    break;
                                }
                            }
                        }   
                    }
                }
                connection.Close();
            }
            return flag;
        }

        public List<Ride> GetAvailableRides(RideRequest request)
        {
            List<Ride> rides = new List<Ride>();
            string connectionString = Configuration.ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = $"select * from Ride where ProviderId!='{request.RiderId}' and NoOfOfferedSeats>='{request.NoOfSeats}'";
                SqlCommand command = new SqlCommand(sql, connection);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        Ride ride = new Ride
                        {
                            Id = Convert.ToInt32(dataReader["Id"]),
                            NoOfOfferedSeats = Convert.ToInt32(dataReader["NoOfOfferedSeats"]),
                            ProviderId = Convert.ToString(dataReader["PRoviderId"]),
                            StartDate = Convert.ToDateTime(dataReader["StartDate"]),
                            UnitDistanceCost = Convert.ToInt32(dataReader["UnitDistanceCost"]),
                            VehicleId = Convert.ToString(dataReader["VehicleId"])
                        };
                        ride.VehicleType = VehicleDal.GetVehicleType(ride.VehicleId);
                        ride.ProviderName = UserDal.GetUserName(ride.ProviderId);
                        List<string> locations = Convert.ToString(dataReader["Locations"]).Split(",").ToList();
                        List<TimeSpan> durations = Convert.ToString(dataReader["Durations"]).Split(",").ToList().ConvertAll(TimeSpan.Parse);
                        TimeSpan pickUpdur = durations[locations.FindIndex(x => x == request.PickUp)];
                        if (ride.VehicleType == request.VehicleType && ride.StartDate.Date == request.StartDate.Date && IsSeatAvailable(request, ride.Id) && IsEnRoute(ride.Id, request.PickUp, request.Drop) && DateTime.Now < (ride.StartDate + pickUpdur))
                        {
                            rides.Add(ride);
                        }
                    }
                }
                connection.Close();
            }
            return rides;
        }

        public float GetCost(int rideId,string source,string destination)
        {
            string connectionString = Configuration.ConnectionString;
            List<string> locations;
            List<float> distances;
            float res=0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = $"select locations,distances,UnitDistanceCost from Ride where Id='{rideId}'";
                SqlCommand command = new SqlCommand(sql, connection);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        locations = Convert.ToString(dataReader["locations"]).Split(",").ToList();
                        distances = Convert.ToString(dataReader["distances"]).Split(",").ToList().ConvertAll(float.Parse);
                        float cost = float.Parse(Convert.ToString(dataReader["UnitDistanceCost"]));
                        res = distances[locations.FindIndex(x => x == destination)] - distances[locations.FindIndex(x => x == source)] * cost;
                    }
                }
                connection.Close();
            }
            return res;
        }

        public bool IsEnRoute(int rideId, string pickUp, string drop)
        {
            string connectionString = Configuration.ConnectionString;
            string locations;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"select locations from Ride where Id='{rideId}'";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    locations = command.ExecuteScalar().ToString();
                    connection.Close();
                }
            }
            return locations.IndexOf(pickUp) < locations.IndexOf(drop);
        }

        public bool IsBooked(int rideId, string userId)
        {
            bool result;
            string connectionString = Configuration.ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = $"Select * From Booking where RideId='{rideId}' and RiderId='{userId}'";
                SqlCommand command = new SqlCommand(sql, connection);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    result = dataReader.HasRows;
                }
                connection.Close();
            }
            return result;
        }

    }
}
