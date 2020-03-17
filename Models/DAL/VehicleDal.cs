using Models.DAL.AppConfig;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Models.DAL
{
    class VehicleDal
    {
        private readonly AppConfiguration Configuration;

        public VehicleDal(AppConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void Create(Vehicle vehicle,string id)
        {
            string connectionString = Configuration.ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"Insert Into Vehicle (Number, Model, Capacity, Type , OwnerId) Values ('{vehicle.Number}','{vehicle.Model}','{vehicle.Capacity}','{vehicle.Type}','{id}')";
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
                    vehicle.Capacity = Convert.ToInt32(dataReader["Capacity"]);
                    vehicle.Model = Convert.ToString(dataReader["Model"]);
                    vehicle.Type = Enum.Parse<VehicleType>(Convert.ToString(dataReader["Type"]));
                    vehicle.Number = id;
                }
                connection.Close();
            }
            return vehicle;
        }

        public List<Vehicle> GetVehiclesByUserId(string id)
        {
            List<Vehicle> vehicles = new List<Vehicle>();
            string connectionString = Configuration.ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "Select * From Teacher";
                SqlCommand command = new SqlCommand(sql, connection);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        Vehicle vehicle = new Vehicle();
                        vehicle.Capacity = Convert.ToInt32(dataReader["Capacity"]);
                        vehicle.Model = Convert.ToString(dataReader["Model"]);
                        vehicle.Type = Enum.Parse<VehicleType>(Convert.ToString(dataReader["Type"]));
                        vehicle.Number = id;
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
                    if (dataReader.HasRows)
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
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
                string sql = $"Select Id From Vehicle where OwnerId='{userId}'";
                SqlCommand command = new SqlCommand(sql, connection);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    vehiclesId.Add(Convert.ToString(dataReader["Id"]));
                }
                connection.Close();
            }
            return vehiclesId;
        }

    }
}
