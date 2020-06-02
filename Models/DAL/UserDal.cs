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

        public UserResponse Create(User user)
        {
            UserResponse userResponse = new UserResponse();
            string connectionString = Configuration.ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
               //string sql = $"Insert Into [User] (Name, Age, Gender, Id, Mail, Password, MobileNumber,Photo) Values ('{user.Name}', '{user.Age}', '{user.Gender}', '{user.Mail}', '{user.Mail}', '{user.Password}', '{user.Number}',@Photo)";
                string sql = $"Insert Into [User] (Name, Age, Gender, Id, Mail, Password, MobileNumber{(user.Photo == null ? "" : ",Photo")}) Values ('{user.Name}', '{user.Age}', '{user.Gender}', '{user.Mail}', '{user.Mail}', '{user.Password}', '{user.Number}'{(user.Photo == null ? "" : ",@Photo")})";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    try
                    {
                        command.CommandType = CommandType.Text;
                        if (user.Photo != null)
                        {
                            command.Parameters.AddWithValue("@Photo", user.Photo);
                        }
                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                    catch (SqlException ex)
                    {
                        if (ex.Message.Contains("Violation of PRIMARY KEY constraint"))
                        {
                            userResponse.ErrorMessage = "User Already Exists";
                        }
                        else
                        {
                            userResponse.ErrorMessage = ex.Message;
                        }
                    }
                    
                }
            }
            user.Wallet.Balance = 0;
            user.Photo = null;
            user.Role = "User";
            userResponse.User = user;
            return userResponse;
        }

        public User Update(User user)
        {
            string connectionString = Configuration.ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"Update [User] set Name='{user.Name}', Age='{user.Age}', Gender='{user.Gender}', Mail='{user.Mail}', Password='{user.Password}', MobileNumber='{user.Number}'{(user.Photo == null ? "" : ",Photo=@Photo")} where id='{user.Mail}'";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    if (user.Photo != null)
                    {
                        command.Parameters.AddWithValue("@Photo", user.Photo);
                    }
                    //command.Parameters.AddWithValue("@Photo", user.Photo);
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return GetById(user.Mail);
        }        

        public bool UpdatePassword(UpdatePassword updatePassword,string userId)
        {
            int noOfEffectedRows;
            string connectionString = Configuration.ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"Update [User] set Password='{updatePassword.NewPassword}' where id='{userId}' and password='{updatePassword.Password}'";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    noOfEffectedRows=command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return noOfEffectedRows == 1;
        }

        public User GetById(string id)
        {
            User user = new User();
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
                        user.Role = Convert.ToString(dataReader["Role"]);
                        //user.Photo = (byte[])dataReader["Photo"];
                        
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
                        User user = new User
                        {
                            Name = Convert.ToString(dataReader["Name"]),
                            Mail = Convert.ToString(dataReader["Mail"]),
                            Age = Convert.ToInt32(dataReader["Age"]),
                            Number = Convert.ToString(dataReader["MobileNumber"]),
                            Gender = Enum.Parse<Gender>(Convert.ToString(dataReader["Gender"])),
                            Photo=(byte[])dataReader["Photo"],
                    };
                        user.Wallet.Balance = Convert.ToInt32(dataReader["Balance"]);
                        users.Add(user);
                    }
                }
                connection.Close();
            }
            return users;
        }

        public UserResponse Login(string id, string password)
        {
            UserResponse response = new UserResponse();
            User user=null;
            string connectionString = Configuration.ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = $"Select * From [User] where Id='{id}'";
                SqlCommand command = new SqlCommand(sql, connection);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            user = new User
                            {
                                Name = Convert.ToString(dataReader["Name"]),
                                Mail = Convert.ToString(dataReader["Mail"]),
                                Password = Convert.ToString(dataReader["Password"]),
                                Age = Convert.ToInt32(dataReader["Age"]),
                                Gender = Enum.Parse<Gender>(Convert.ToString(dataReader["Gender"])),
                                Number = Convert.ToString(dataReader["MobileNumber"]),
                                Photo = Convert.IsDBNull(dataReader["Photo"]) ? null: (byte[])dataReader["Photo"],
                                //Photo=null,
                                Role=Convert.ToString(dataReader["Role"]),
                            };
                            user.Wallet.Balance = Convert.ToInt32(dataReader["Balance"]);
                        }
                        if (user.Password != password)
                        {
                            response.ErrorMessage = "Invalid Password";
                        }
                        else
                        {
                            response.User = user;
                        }
                    }
                    else
                    {
                        response.ErrorMessage = "Invalid Id";
                    }
                }
                connection.Close();
            }
            return response;
        }

        public void AddBalance(float amount,string id)
        {
            string connectionString = Configuration.ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"Update [User] set Balance=Balance+'{amount}' where id='{id}'";
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

        public byte[] GetImage(string userId)
        {
            string connectionString = Configuration.ConnectionString;
            object Photo;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"Select Photo from [User] Where Id='{userId}'";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    Photo = command.ExecuteScalar();
                    connection.Close();
                }
                connection.Close();
            }
            try
            {
                return (byte[])Photo;
            }
            catch
            {
                return null;
            }
        }

        public byte[] UpdateImage(byte[] photo,string userId)
        {
            string connectionString = Configuration.ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"Update [User] set Photo=@Photo where id='{userId}'";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@Photo", photo);
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            
            object Photo;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"Select Photo from [User] Where Id='{userId}'";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    Photo = command.ExecuteScalar();
                    connection.Close();
                }
                connection.Close();
            }
            try
            {
                return (byte[])Photo;
            }
            catch
            {
                return null;
            }
        }

    }
}
