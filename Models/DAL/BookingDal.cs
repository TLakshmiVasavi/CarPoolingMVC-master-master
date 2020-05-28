using Models.DAL.AppConfig;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Models.DAL
{
    public class BookingDal
    {
        private readonly AppConfiguration Configuration;
        RideDal RideDal = new RideDal(new AppConfiguration());
        UserDal UserDal = new UserDal(new AppConfiguration());

        public BookingDal(AppConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void Create(RideRequest request)
        {
            float cost = RideDal.GetCost(request.RideId, request.From, request.To);
            string vehicleNumber = RideDal.GetRideVehicle(request.RideId);
            string connectionString = Configuration.ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"Insert Into Booking (RideId,RiderId,Source,Destination,NoOfSeats,StartDate,Time,Cost) Values('{request.RideId}','{request.RiderId}','{request.From}','{request.To}','{request.NoOfSeats}','{request.StartDate}','{request.Time}','{cost}')";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public List<Booking> GetAllBookings()
        {
            string connectionString = Configuration.ConnectionString;
            List<Booking> bookings = new List<Booking>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"Select * from Booking";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            Booking booking = new Booking
                            {
                                Request = new RideRequest(),
                                Response = new BookRideResponse()
                            };
                            booking.Request.From = Convert.ToString(dataReader["Source"]);
                            booking.Request.To = Convert.ToString(dataReader["Destination"]);
                            booking.Request.NoOfSeats = Convert.ToInt32(dataReader["NoOfSeats"]);
                            booking.Request.Id = Convert.ToInt32(dataReader["Id"]);
                            booking.Request.RiderId = Convert.ToString(dataReader["RiderId"]);
                            booking.Request.StartDate = Convert.ToDateTime(dataReader["StartDate"]);
                            booking.Request.Time = Convert.ToString(dataReader["Time"]);
                            booking.Response.Status = Enum.Parse<Status>(Convert.ToString(dataReader["Status"]));
                            booking.Response.Cost = float.Parse(Convert.ToString(dataReader["Cost"]));
                            bookings.Add(booking);
                        }
                    }
                }
                connection.Close();
            }
            return bookings;
        }

        public Booking GetBooking(int id)
        {
            string connectionString = Configuration.ConnectionString;
            Booking booking = new Booking
            {
                Request = new RideRequest(),
                Response = new BookRideResponse()
            };
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"Select * from Booking where Id='{id}'";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            booking.Request.From = Convert.ToString(dataReader["Source"]);
                            booking.Request.To = Convert.ToString(dataReader["Destination"]);
                            booking.Request.NoOfSeats = Convert.ToInt32(dataReader["NoOfSeats"]);
                            booking.Request.Id = Convert.ToInt32(dataReader["Id"]);
                            booking.Request.RiderId = Convert.ToString(dataReader["RiderId"]);
                            booking.Request.StartDate = Convert.ToDateTime(dataReader["StartDate"]);
                            booking.Response.Status = Enum.Parse<Status>(Convert.ToString(dataReader["Status"]));
                            booking.Response.Cost = float.Parse(Convert.ToString(dataReader["Cost"]));
                        }
                    }
                }
                connection.Close();
            }
            return booking;
        }

        public void Update(RideRequest request, float cost)
        {
            string connectionString = Configuration.ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"Update Booking RideId='{request.RideId}',RiderId='{request.RiderId}',Source='{request.From}',Destination='{request.To}',NoOfSeats='{request.NoOfSeats}',StartDate='{request.StartDate}',Cost='{cost}'";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public void UpdateStatus(int requestId,string status)
        {
            string connectionString = Configuration.ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"Update Booking set Status='{status}' Where Id='{requestId}'";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                connection.Close();
            }
        }

        public Booking GetBookingDetails(int id)
        {
            string connectionString = Configuration.ConnectionString;
            Booking booking = new Booking
            {
                Request = new RideRequest(),
                Response = new BookRideResponse()
            };

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"Select * from Booking Where Id='{id}'";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            booking.Request.From = Convert.ToString(dataReader["Source"]);
                            booking.Request.To = Convert.ToString(dataReader["Destination"]);
                            booking.Request.NoOfSeats = Convert.ToInt32(dataReader["NoOfSeats"]);
                            booking.Request.Id = Convert.ToInt32(dataReader["Id"]);
                            booking.Request.RiderId = Convert.ToString(dataReader["RiderId"]);
                            booking.Request.StartDate = Convert.ToDateTime(dataReader["StartDate"]);
                            booking.Response.Status = Enum.Parse<Status>(Convert.ToString(dataReader["Status"]));
                            booking.Response.Cost = float.Parse(Convert.ToString(dataReader["Cost"]));
                            booking.Response.VehicleNumber = Convert.ToString(dataReader["VehicleNumber"]);
                            
                        }
                    }
                    connection.Close();
                }
                connection.Close();
            }
            return booking;
        }

        public List<Booking> GetBookings(string userId)
        {
            string connectionString = Configuration.ConnectionString;
            List<Booking> bookings = new List<Booking>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"Select * from Booking where RiderId='{userId}'";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            Booking booking = new Booking
                            {
                                Request = new RideRequest(),
                                Response = new BookRideResponse()
                            };
                            booking.Request.From = Convert.ToString(dataReader["Source"]);
                            booking.Request.To = Convert.ToString(dataReader["Destination"]);
                            booking.Request.NoOfSeats = Convert.ToInt32(dataReader["NoOfSeats"]);
                            booking.Request.Id = Convert.ToInt32(dataReader["Id"]);
                            booking.Request.RiderId = Convert.ToString(dataReader["RiderId"]);
                            booking.Request.StartDate = Convert.ToDateTime(dataReader["StartDate"]);
                            booking.Response.Status = Enum.Parse<Status>(Convert.ToString(dataReader["Status"]));
                            booking.Response.Cost = float.Parse(Convert.ToString(dataReader["Cost"]));
                            bookings.Add(booking);
                        }
                    }
                }
                connection.Close();
            }
            return bookings;
        }

        public Status GetStatus(int id)
        {
            string connectionString = Configuration.ConnectionString;
            string type;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"select Status from Booking where Id='{id}'";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    type = command.ExecuteScalar().ToString();
                    connection.Close();
                }
            }
            return Enum.Parse<Status>(type);
        }

        public List<RequestDetails> GetRequests(int rideId)
        {
            string connectionString = Configuration.ConnectionString;
            List<RequestDetails> requests = new List<RequestDetails>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"Select * from Booking where RideId='{rideId}'";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            RequestDetails Request = new RequestDetails
                            {
                                From = Convert.ToString(dataReader["Source"]),
                                To = Convert.ToString(dataReader["Destination"]),
                                NoOfSeats = Convert.ToInt32(dataReader["NoOfSeats"]),
                                Id = Convert.ToInt32(dataReader["Id"]),
                                StartDate = Convert.ToDateTime(dataReader["StartDate"])
                            };
                            string RiderId = Convert.ToString(dataReader["RiderId"]);
                            Request.RiderName = UserDal.GetUserName(RiderId);
                            Request.RiderPic = UserDal.GetImage(RiderId);
                            Request.Cost = RideDal.GetCost(rideId, Request.From, Request.To);
                            requests.Add(Request);
                        }
                    }
                }
                connection.Close();
            }
            return requests;
        }

        public RideRequest GetRequest(int bookingId)
        {
            string connectionString = Configuration.ConnectionString;
            RideRequest request=new RideRequest();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"Select * from Booking where Id='{bookingId}'";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            request.From = Convert.ToString(dataReader["Source"]);
                            request.To = Convert.ToString(dataReader["Destination"]);
                            request.NoOfSeats = Convert.ToInt32(dataReader["NoOfSeats"]);
                            request.Id = Convert.ToInt32(dataReader["Id"]);
                            request.RiderId = Convert.ToString(dataReader["RiderId"]);
                            request.StartDate = Convert.ToDateTime(dataReader["StartDate"]);
                        }
                    }
                }
                connection.Close();
            }
            return request;
        }

    }
}
