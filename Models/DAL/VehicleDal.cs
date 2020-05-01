using Models.DAL.AppConfig;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Models.DAL
{
    public class VehicleDal
    {
        private readonly AppConfiguration Configuration;

        public VehicleDal(AppConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void Create(Vehicle vehicle,string userId)
        {
            string connectionString = Configuration.ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"Insert Into Vehicle (Number, Model, Capacity, Type , OwnerId) Values ('{vehicle.Number}','{vehicle.Model}','{vehicle.Capacity}','{vehicle.Type}','{userId}')";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public void Update(Vehicle vehicle,string userId)
        {
            string connectionString = Configuration.ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"Update Vehicle set Model='{vehicle.Model}', Capacity='{vehicle.Capacity}', Type='{vehicle.Type}' , OwnerId='{userId}' where Number='{vehicle.Number}' ";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public Vehicle GetById(string id)
        {
            Vehicle vehicle = new Vehicle();
            string connectionString = Configuration.ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = $"Select * From Vehicle where Number='{id}'";
                SqlCommand command = new SqlCommand(sql, connection);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        vehicle.Capacity = Convert.ToInt32(dataReader["Capacity"]);
                        vehicle.Model = Convert.ToString(dataReader["Model"]);
                        vehicle.Type = Enum.Parse<VehicleType>(Convert.ToString(dataReader["Type"]));
                        vehicle.Number = id;
                    }
                }
                connection.Close();
            }
            return vehicle;
        }

        public List<Vehicle> GetAllVehicles()
        {
            List<Vehicle> vehicles = new List<Vehicle>();
            string connectionString = Configuration.ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = $"Select * From Vehicle";
                SqlCommand command = new SqlCommand(sql, connection);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        Vehicle vehicle = new Vehicle
                        {
                            Capacity = Convert.ToInt32(dataReader["Capacity"]),
                            Model = Convert.ToString(dataReader["Model"]),
                            Type = Enum.Parse<VehicleType>(Convert.ToString(dataReader["Type"])),
                            Number = Convert.ToString(dataReader["Number"])
                        };
                        vehicles.Add(vehicle);
                    }
                }
                connection.Close();
            }
            return vehicles;
        }

        public List<Vehicle> GetVehicles(string userId)
        {
            List<Vehicle> vehicles = new List<Vehicle>();
            string connectionString = Configuration.ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = $"Select * From Vehicle where OwnerId='{userId}'";
                SqlCommand command = new SqlCommand(sql, connection);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        Vehicle vehicle = new Vehicle
                        {
                            Capacity = Convert.ToInt32(dataReader["Capacity"]),
                            Model = Convert.ToString(dataReader["Model"]),
                            Type = Enum.Parse<VehicleType>(Convert.ToString(dataReader["Type"])),
                            Number = userId
                        };
                        vehicles.Add(vehicle);
                    }
                }
                connection.Close();
            }
            return vehicles;
        }

        public bool HasVehicle(string userId)
        {
            bool result;
            string connectionString = Configuration.ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = $"Select * From Vehicle where OwnerId='{userId}'";
                SqlCommand command = new SqlCommand(sql, connection);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    result = dataReader.HasRows;
                }
                connection.Close();
            }
            return result;
        }

        public List<string> GetVehiclesId(string userId)
        {
            List<string> vehiclesId=new List<string>();
            string connectionString = Configuration.ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = $"Select Number From Vehicle where OwnerId='{userId}'";
                SqlCommand command = new SqlCommand(sql, connection);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        vehiclesId.Add(Convert.ToString(dataReader["Number"]));
                    }
                }
                connection.Close();
            }
            return vehiclesId;
        }

        public VehicleType GetVehicleType(string vehicleId)
        {
            string connectionString = Configuration.ConnectionString;
            string type;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"select Type from Vehicle where Number='{vehicleId}'";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    type = command.ExecuteScalar().ToString();
                    connection.Close();
                }
            }
            return Enum.Parse<VehicleType>(type);
        }

    }
}
