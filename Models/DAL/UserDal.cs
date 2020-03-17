using Models.DAL.AppConfig;
using Models.Enums;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Models.DAL
{
    public class UserDal
    {
        private readonly AppConfiguration Configuration;

        public UserDal(AppConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void Create(User user)
        {
            string connectionString = Configuration.ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"Insert Into [User] (Name, Age, Gender, Id, Mail, Password, Number) Values ('{user.Name}', '{user.Age}', '{user.Gender}', '{user.Id}', '{user.Mail}', '{user.Password}', '{user.Number}')";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public User GetById(string id)
        {
            User user = new User(); 
            string connectionString = Configuration.ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = $"Select * From User where Id='{id}'";
                SqlCommand command = new SqlCommand(sql, connection);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        user.Name = Convert.ToString(dataReader["Name"]);
                        user.Mail = Convert.ToString(dataReader["Mail"]);
                        user.Age = Convert.ToInt32(dataReader["Age"]);
                        user.Wallet.Balance = Convert.ToInt32(dataReader["Balance"]);
                        user.Gender = Enum.Parse<Gender>(Convert.ToString(dataReader["Gender"]));
                        user.Number = Convert.ToString(dataReader["Number"]);
                    }
                }
                connection.Close();
            }
            return user;
        }

        public bool Login(string id, string password)
        {
            bool result;
            string connectionString = Configuration.ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = $"Select * From User where Id='{id}' and Password='{password}'";
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

        public void AddBalance(float amount,string id)
        {
            string connectionString = Configuration.ConnectionString;
            User user = new User();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"Update User set Balance=Balance+'{amount}' Where Id='{id}'";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                connection.Close();
            }
        }

        public float GetBalance(string id)
        {
            string connectionString = Configuration.ConnectionString;
            float balance;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"Select Balance from User Where Id='{id}'";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {
                        balance = float.Parse(dataReader["Balance"].ToString());
                    }
                    connection.Close();
                }
                connection.Close();
            }
            return balance;
        }

    }
}
