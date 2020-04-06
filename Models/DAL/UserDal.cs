using Models.DAL.AppConfig;
using Models.Enums;
using System;
using System.Collections.Generic;
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
                string sql = $"Insert Into [User] (Name, Age, Gender, Id, Mail, Password, MobileNumber,Photo) Values ('{user.Name}', '{user.Age}', '{user.Gender}', '{user.Mail}', '{user.Mail}', '{user.Password}', '{user.Number}',@Photo)";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@Photo", user.Photo);
                    connection.Open();
                    //command.Parameters.AddWithValue("Photo",user.Photo);
                    //command.Parameters.Add("Photo",user.Photo);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public void Update(User user)
        {
            string connectionString = Configuration.ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"Update [User] set Name='{user.Name}', Age='{user.Age}', Gender='{user.Gender}', Mail='{user.Mail}', Password='{user.Password}', MobileNumber='{user.Number}' where id='{user.Id}'";

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
            user.Wallet=new Wallet();
            string connectionString = Configuration.ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = $"Select * From [User] where Id='{id}'";
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
                        user.Number = Convert.ToString(dataReader["MobileNumber"]);
                        user.Photo = (byte[])dataReader["Photo"];
                        
                    }
                }
                connection.Close();
            }
            return user;
        }

        public List<User> GetAllUsers()
        {
            List<User> users = new List<User>();
            string connectionString = Configuration.ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = $"Select * From [User]";
                SqlCommand command = new SqlCommand(sql, connection);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        User user = new User();
                        user.Name = Convert.ToString(dataReader["Name"]);
                        user.Mail = Convert.ToString(dataReader["Mail"]);
                        user.Age = Convert.ToInt32(dataReader["Age"]);
                        user.Wallet.Balance = Convert.ToInt32(dataReader["Balance"]);
                        user.Gender = Enum.Parse<Gender>(Convert.ToString(dataReader["Gender"]));
                        user.Number = Convert.ToString(dataReader["Number"]);
                        users.Add(user);
                    }
                }
                connection.Close();
            }
            return users;
        }

        public bool Login(string id, string password)
        {
            bool result;
            string connectionString = Configuration.ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = $"Select * From [User] where Id='{id}' and Password='{password}'";
                SqlCommand command = new SqlCommand(sql, connection);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    result = dataReader.HasRows;
                }
                connection.Close();
            }
            return result;
        }

        public void AddBalance(float amount,string id)
        {
            string connectionString = Configuration.ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"Update [User] set Balance=Balance+'{amount}' Where Id='{id}'";
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
                string sql = $"Select Balance from [User] Where Id='{id}'";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    balance = float.Parse(command.ExecuteScalar().ToString());
                    connection.Close();
                }
                connection.Close();
            }
            return balance;
        }

        public bool IsUserExist(string userId)
        {
            bool result;
            string connectionString = Configuration.ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = $"Select * From [User] where Id='{userId}'";
                SqlCommand command = new SqlCommand(sql, connection);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    result = dataReader.HasRows;
                }
                connection.Close();
            }
            return result;
        }

        public string GetUserName(string userId)
        {
            string connectionString = Configuration.ConnectionString;
            string name;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"Select Name from [User] Where Id='{userId}'";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    name = Convert.ToString(command.ExecuteScalar());
                    connection.Close();
                }
                connection.Close();
            }
            return name;
        }

    }
}
