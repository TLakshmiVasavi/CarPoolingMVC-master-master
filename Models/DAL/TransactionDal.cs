using Models;
using Models.DAL.AppConfig;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Models.DAL
{
    public class TransactionDal
    {
        private readonly AppConfiguration Configuration;

        public TransactionDal(AppConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void Create(Transaction transaction)
        {
            transaction.Id = Guid.NewGuid();
            string connectionString = Configuration.ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"Insert Into [Transaction] (Id,BookingId,Amount,Sender,Receiver) Values ('{transaction.Id}','{transaction.BookingId}','{transaction.Amount}','{transaction.From}','{transaction.To}')";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                
            }

        }

        public Transaction GetById(Guid guid)
        {
            Transaction transaction = new Transaction();
            string connectionString = Configuration.ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = $"Select * From [Transaction] where Id='{guid}'";
                SqlCommand command = new SqlCommand(sql, connection);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        transaction.BookingId = Convert.ToInt32(dataReader["BookingId"]);
                        transaction.Amount = float.Parse(Convert.ToString(dataReader["Amount"]));
                        transaction.From = Convert.ToString(dataReader["Sender"]);
                        transaction.To = Convert.ToString(dataReader["Receiver"]);
                    }
                }
                connection.Close();
            }
            return transaction;
        }

        public List<Transaction> GetTransactionsByUserId(string userId)
        {
            List<Transaction> transactions = new List<Transaction>();
            string connectionString = Configuration.ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = $"Select * From [Transaction] where Sender='{userId}' OR Receiver='{userId}'";
                SqlCommand command = new SqlCommand(sql, connection);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    Transaction transaction = new Transaction();
                    while (dataReader.Read())
                    {
                        transaction.Id = (Guid)dataReader["Id"];
                        transaction.BookingId = Convert.ToInt32(dataReader["BookingId"]);
                        transaction.Amount = float.Parse(Convert.ToString(dataReader["Amount"]));
                        transaction.From = Convert.ToString(dataReader["Sender"]);
                        transaction.To = Convert.ToString(dataReader["Receiver"]);
                    }
                    transactions.Add(transaction);
                }
                connection.Close();
            }
            return transactions;
        }
    }
}
