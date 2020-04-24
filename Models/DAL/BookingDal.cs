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

        public BookingDal(AppConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void Create(RideRequest request)
        {
            float cost = RideDal.GetCost(request.RideId, request.PickUp, request.Drop);
            string connectionString = Configuration.ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"Insert Into Booking (RideId,RiderId,Source,Destination,NoOfSeats,StartDate,Cost) Values('{request.RideId}','{request.RiderId}','{request.PickUp}','{request.Drop}','{request.NoOfSeats}','{request.StartDate}','{cost}')";
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
                            booking.Request.PickUp = Convert.ToString(dataReader["Source"]);
                            booking.Request.Drop = Convert.ToString(dataReader["Destination"]);
                            booking.Request.NoOfSeats = Convert.ToInt32(dataReader["NoOfSeats"]);
                            booking.Request.Id = Convert.ToString(dataReader["Id"]);
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
                            booking.Request.PickUp = Convert.ToString(dataReader["Source"]);
                            booking.Request.Drop = Convert.ToString(dataReader["Destination"]);
                            booking.Request.NoOfSeats = Convert.ToInt32(dataReader["NoOfSeats"]);
                            booking.Request.Id = Convert.ToString(dataReader["Id"]);
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
                string sql = $"Update Booking RideId='{request.RideId}',RiderId='{request.RiderId}',Source='{request.PickUp}',Destination='{request.Drop}',NoOfSeats='{request.NoOfSeats}',StartDate='{request.StartDate}',Cost='{cost}'";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public void UpdateStatus(string requestId,string status)
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

        public Booking GetBookingDetails(string id)
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
                            booking.Request.PickUp = Convert.ToString(dataReader["Source"]);
                            booking.Request.Drop = Convert.ToString(dataReader["Destination"]);
                            booking.Request.NoOfSeats = Convert.ToInt32(dataReader["NoOfSeats"]);
                            booking.Request.Id = Convert.ToString(dataReader["Id"]);
                            booking.Request.RiderId = Convert.ToString(dataReader["RiderId"]);
                            booking.Request.StartDate = Convert.ToDateTime(dataReader["StartDate"]);
                            booking.Response.Status = Enum.Parse<Status>(Convert.ToString(dataReader["Status"]));
                            booking.Response.Cost = float.Parse(Convert.ToString(dataReader["Cost"]));
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
                            booking.Request.PickUp = Convert.ToString(dataReader["Source"]);
                            booking.Request.Drop = Convert.ToString(dataReader["Destination"]);
                            booking.Request.NoOfSeats = Convert.ToInt32(dataReader["NoOfSeats"]);
                            booking.Request.Id = Convert.ToString(dataReader["Id"]);
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

        public Status GetStatus(string id)
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

        public List<RideRequest> GetRequests(int rideId)
        {
            string connectionString = Configuration.ConnectionString;
            List<RideRequest> requests = new List<RideRequest>();
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
                            RideRequest Request = new RideRequest
                            {
                                PickUp = Convert.ToString(dataReader["Source"]),
                                Drop = Convert.ToString(dataReader["Destination"]),
                                NoOfSeats = Convert.ToInt32(dataReader["NoOfSeats"]),
                                Id = Convert.ToString(dataReader["Id"]),
                                RiderId = Convert.ToString(dataReader["RiderId"]),
                                StartDate = Convert.ToDateTime(dataReader["StartDate"])
                            };
                            requests.Add(Request);
                        }
                    }
                }
                connection.Close();
            }
            return requests;
        }

    }
}
