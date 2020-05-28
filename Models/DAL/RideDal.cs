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
                string locations = ride.Route.From + "," + string.Join(",", ride.Route.Stops.Select(_ => _.Location).ToList()) + ((ride.Route.Stops.Count > 0) ? "," : "") + ride.Route.To;
                string distances = string.Join(",", ride.Route.Stops.Select(_ => _.Distance).ToList()) + ((ride.Route.Stops.Count > 0) ? "," : "") + ride.Distance;
                //                string durations = string.Join(",", ride.Route.ViaPoints.Select(_ => _.Duration).ToList()) + ((ride.Route.ViaPoints.Count > 0) ? "," : "") + (ride.EndDateTime - ride.StartDate);
                string durations = string.Join(",", ride.Route.Stops.Select(_ => _.Duration).ToList()) + ((ride.Route.Stops.Count > 0) ? "," : "") + (ride.StartDate);
                string sql = $"Insert Into Ride (VehicleId , ProviderId " +
                    $", NoOfOfferedSeats , UnitDistanceCost , StartDate ,Time, Locations ," +
                    $" Distances , Durations ,Duration ,Distance ) Values ('{ride.VehicleId}','{ride.ProviderId}'," +
                    $"'{ride.NoOfOfferedSeats}','{ride.Cost}','{ride.StartDate}','{ride.Time}','{locations}'," +
                    $"'{distances}','{durations}','{ride.StartDate}','{ride.Distance}')";//
                //string sql = $"Insert Into Ride (VehicleId, ProviderId, NoOfOfferedSeats, UnitDistanceCost, StartDate,Time, Locations," +
                //$" Distances, Durations,Duration,Distance) Values ('{ride.VehicleId}','{ride.ProviderId}'," +
                //$"'{ride.NoOfOfferedSeats}','{ride.UnitDistanceCost}','{ride.StartDate}','{locations}'," +
                //$"'{distances}','{durations}','{ride.EndDateTime - ride.StartDate}','{ride.Distance}')";
                //string sql = $"Insert Into Ride (VehicleId, ProviderId, NoOfOfferedSeats, UnitDistanceCost, StartDate,Time, Distance" +
                //    $"'{ride.VehicleId}','{ride.ProviderId}','{ride.NoOfOfferedSeats}','{ride.UnitDistanceCost}','{ride.StartDate}','{ride.Time}','{ride.Distance}'";
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
                string locations = ride.Route.From + "," + string.Join(",", ride.Route.Stops.Select(_ => _.Location).ToList()) + ((ride.Route.Stops.Count > 0) ? "," : "") + ride.Route.To;
                string distances = string.Join(",", ride.Route.Stops.Select(_ => _.Distance).ToList()) + ((ride.Route.Stops.Count > 0) ? "," : "") + ride.Distance;
                //                string durations = string.Join(",", ride.Route.ViaPoints.Select(_ => _.Duration).ToList()) + ((ride.Route.ViaPoints.Count > 0) ? "," : "") + (ride.EndDateTime - ride.StartDate);
                string durations = string.Join(",", ride.Route.Stops.Select(_ => _.Duration).ToList()) + ((ride.Route.Stops.Count > 0) ? "," : "") + (ride.StartDate);
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
                            From = locations.First(),
                            To = locations.Last(),
                            Stops = new List<Stop>()
                        };
                        locations.RemoveAt(0);
                        locations.RemoveAt(locations.Count - 1);
                        locations.ForEach(_ => Ride.Route.Stops.Add(new Stop() { Location = _ }));
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
                            Time = Convert.ToString(dataReader["Time"]),
                            Distance=Convert.ToInt32(dataReader["Distance"])
                        };
                        locations = Convert.ToString(dataReader["locations"]).Split(",").ToList();
                        Ride.VehicleType = VehicleDal.GetVehicleType(Ride.VehicleId);
                        Ride.ProviderName = UserDal.GetUserName(Ride.ProviderId);
                        Ride.Route = new Route()
                        {
                            From = locations.First(),
                            To = locations.Last(),
                            Stops = new List<Stop>()
                        };
                        Ride.Cost = Ride.UnitDistanceCost * Ride.Distance;
                        Ride.Status = "UpComing";
                        if (Ride.StartDate.Date == DateTime.Now.Date)
                        {
                            int time = Ride.Time[1] == 'a' ? Convert.ToInt32(Ride.Time[0]-'0') : Convert.ToInt32(Ride.Time[0]-'0') + 12;
                            if (DateTime.Now.Hour > time ) {
                                Ride.Status = "Completed"; }
                            else
                            {
                                Ride.Status = "UpComping";
                            }
                        }
                        else
                        if(Ride.StartDate<DateTime.Now.Date)
                        {
                            Ride.Status = "Completed";
                        }
                        locations.RemoveAt(0);
                        locations.RemoveAt(locations.Count - 1);
                        locations.ForEach(_ => Ride.Route.Stops.Add(new Stop() { Location = _ }));
                        rides.Add(Ride);
                    }
                }
                connection.Close();
            }
            return rides;
        }

        public int GetNoOfAvailableSeats(RideRequest request, int rideId)
        {
            int AvaliableSeats = 1;
            List<string> locations = new List<string>();
            List<float> distances = new List<float>();
            List<PlaceDetails> placesDetails = new List<PlaceDetails>();
            string connectionString = Configuration.ConnectionString;
            int NoOfOfferedSeats = 0;
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
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            int noOfSeats = Convert.ToInt32(dataReader["NoOfOfferedSeats"]);
                            float pickUpdist = distances[locations.FindIndex(x => x == request.From)];
                            float dropdist = distances[locations.FindIndex(x => x == request.To)];

                            do
                            {
                                foreach (var item in placesDetails)
                                {
                                    if (pickUpdist <= item.distance && item.distance < dropdist)
                                    {
                                        item.count += request.NoOfSeats;
                                        if (item.count + noOfSeats > NoOfOfferedSeats)
                                        {
                                            break;
                                        }
                                    }
                                }
                                AvaliableSeats++;
                            } while (true);
                        }
                    }
                    else
                    {
                        AvaliableSeats = NoOfOfferedSeats;
                    }
                }
                connection.Close();
            }
            return AvaliableSeats - 1;
        }

        public bool IsSeatAvailable(RideRequest request, int rideId)
        {
            List<string> locations = new List<string>();
            List<float> distances = new List<float>();
            List<PlaceDetails> placesDetails = new List<PlaceDetails>();
            string connectionString = Configuration.ConnectionString;
            int NoOfOfferedSeats = 0;
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
                        float pickUpdist = distances[locations.FindIndex(x => x == request.From)];
                        float dropdist = distances[locations.FindIndex(x => x == request.To)];
                        foreach (var item in placesDetails)
                        {
                            if (pickUpdist <= item.distance && item.distance < dropdist)
                            {
                                item.count += request.NoOfSeats;
                                if (item.count + noOfSeats > NoOfOfferedSeats)
                                {
                                    flag = false;
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

        public List<RideDetails> BookRide(RideRequest request)
        {
            List<RideDetails> rides = new List<RideDetails>();
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
                        RideDetails ride = new RideDetails
                        {
                            Id = Convert.ToInt32(dataReader["Id"]),
                            ProviderId = Convert.ToString(dataReader["ProviderId"]),
                            StartDate = Convert.ToDateTime(dataReader["StartDate"]),
                            Time = Convert.ToString(dataReader["Time"]),
                            Vehicle=new Vehicle
                            {
                                Number = Convert.ToString(dataReader["VehicleId"])
                            }
                        };
                        ride.Vehicle = VehicleDal.GetById(ride.Vehicle.Number);
                        ride.ProviderName = UserDal.GetUserName(ride.ProviderId);
                        List<string> locations = Convert.ToString(dataReader["Locations"]).Split(",").ToList();
                        //List<TimeSpan> durations = Convert.ToString(dataReader["Durations"]).Split(",").ToList().ConvertAll(TimeSpan.Parse);
                        //TimeSpan pickUpdur = durations[locations.FindIndex(x => x == request.From)];
                        ride.AvailableSeats = GetNoOfAvailableSeats(request, ride.Id);
                        ride.ProviderPic = UserDal.GetImage(ride.ProviderId);
                       if (ride.Time==request.Time && ride.Vehicle.Type == request.VehicleType && ride.AvailableSeats != 0 && ride.StartDate.Date == request.StartDate.Date && IsEnRoute(ride.Id, request.From, request.To))
                        {
                            ride.Cost = GetCost(ride.Id, request.From, request.To);
                            rides.Add(ride);
                        }
                    }
                }
                connection.Close();
            }
            return rides;
        }

        public float GetCost(int rideId, string from, string to)
        {
            string connectionString = Configuration.ConnectionString;
            List<string> locations;
            List<float> distances;
            float res = 0;
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
                        res = distances[locations.FindIndex(x => x == to)] - distances[locations.FindIndex(x => x == from)] * cost;
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

        public string GetRideVehicle(int rideId)
        {
            string connectionString = Configuration.ConnectionString;
            object vehicleNumber;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"Select VehicleNumber From Ride where RideId='{rideId}' ";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    vehicleNumber = command.ExecuteScalar();
                    connection.Close();
                }
                connection.Close();
            }
            return (string)vehicleNumber;
        }
    }
}
