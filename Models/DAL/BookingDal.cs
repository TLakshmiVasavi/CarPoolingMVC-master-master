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

        public BookingDal(AppConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void Create(Request request, int rideId, float cost)
        {
            string connectionString = Configuration.ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"Insert Into Booking (RideId,RiderId,PickUp,Drop,NoOfSeats,StartDate,Cost) Values('{rideId}','{request.RiderId}','{request.PickUp}','{request.Drop}','{request.NoOfSeats}','{request.StartDateTime}','{cost}')";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public List<Booking> GetAllBookings(string userId)
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
                                Request = new Request(),
                                Response = new Response()
                            };
                            booking.Request.PickUp = Convert.ToString(dataReader["PickUp"]);
                            booking.Request.Drop = Convert.ToString(dataReader["Drop"]);
                            booking.Request.NoOfSeats = Convert.ToInt32(dataReader["NoOfSeats"]);
                            booking.Request.RequestId = Convert.ToString(dataReader["Id"]);
                            booking.Request.RiderId = Convert.ToString(dataReader["RiderId"]);
                            booking.Request.StartDateTime = Convert.ToDateTime(dataReader["StartDateTime"]);
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
                Request = new Request(),
                Response = new Response()
            };
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"Select * from Booking where Id='{id}'";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {
                        booking.Request.PickUp = Convert.ToString(dataReader["PickUp"]);
                        booking.Request.Drop = Convert.ToString(dataReader["Drop"]);
                        booking.Request.NoOfSeats = Convert.ToInt32(dataReader["NoOfSeats"]);
                        booking.Request.RequestId = Convert.ToString(dataReader["Id"]);
                        booking.Request.RiderId = Convert.ToString(dataReader["RiderId"]);
                        booking.Request.StartDateTime = Convert.ToDateTime(dataReader["StartDateTime"]);
                        booking.Response.Status = Enum.Parse<Status>(Convert.ToString(dataReader["Status"]));
                        booking.Response.Cost = float.Parse(Convert.ToString(dataReader["Cost"]));
                    }
                }
                connection.Close();
            }
            return booking;
        }

        public void Update(Request request, int rideId, float cost)
        {
            string connectionString = Configuration.ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"Update Booking RideId='{rideId}',RiderId='{request.RiderId}',PickUp='{request.PickUp}',Drop='{request.Drop}',NoOfSeats='{request.NoOfSeats}',StartDate='{request.StartDateTime}',Cost='{cost}'";
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
                Request = new Request(),
                Response = new Response()
            };

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"Select * from Booking Where Id='{id}'";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {
                        booking.Request.PickUp = Convert.ToString(dataReader["PickUp"]);
                        booking.Request.Drop = Convert.ToString(dataReader["Drop"]);
                        booking.Request.NoOfSeats = Convert.ToInt32(dataReader["NoOfSeats"]);
                        booking.Request.RequestId = Convert.ToString(dataReader["Id"]);
                        booking.Request.RiderId = Convert.ToString(dataReader["RiderId"]);
                        booking.Request.StartDateTime = Convert.ToDateTime(dataReader["StartDateTime"]);
                        booking.Response.Status =Enum.Parse<Status>( Convert.ToString(dataReader["Status"]));
                        booking.Response.Cost =float.Parse( Convert.ToString(dataReader["Cost"]));
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
                string sql = $"Select * from Booking Where Id IN (Select Id from Ride where RiderId='{userId}')";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            Booking booking = new Booking
                            {
                                Request = new Request(),
                                Response = new Response()
                            };
                            booking.Request.PickUp = Convert.ToString(dataReader["PickUp"]);
                            booking.Request.Drop = Convert.ToString(dataReader["Drop"]);
                            booking.Request.NoOfSeats = Convert.ToInt32(dataReader["NoOfSeats"]);
                            booking.Request.RequestId = Convert.ToString(dataReader["Id"]);
                            booking.Request.RiderId = Convert.ToString(dataReader["RiderId"]);
                            booking.Request.StartDateTime = Convert.ToDateTime(dataReader["StartDateTime"]);
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

        public List<Request> GetRequests(int rideId)
        {
            string connectionString = Configuration.ConnectionString;
            List<Request> requests = new List<Request>();
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
                            Request Request = new Request();
                            
                            Request.PickUp = Convert.ToString(dataReader["PickUp"]);
                            Request.Drop = Convert.ToString(dataReader["Drop"]);
                            Request.NoOfSeats = Convert.ToInt32(dataReader["NoOfSeats"]);
                            Request.RequestId = Convert.ToString(dataReader["Id"]);
                            Request.RiderId = Convert.ToString(dataReader["RiderId"]);
                            Request.StartDateTime = Convert.ToDateTime(dataReader["StartDateTime"]);
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
