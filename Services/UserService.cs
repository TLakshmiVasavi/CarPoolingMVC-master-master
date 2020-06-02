using Models.Interfaces;
using Models;
using System.Collections.Generic;
using System.Linq;
using Models.DAL;
using Models.DAL.AppConfig;

namespace Services
{
    public class UserService : IUserService
    {
        readonly UserDal userDal = new UserDal(new AppConfiguration());
        readonly VehicleDal vehicleDal = new VehicleDal(new AppConfiguration());
        readonly TransactionDal transactionDal = new TransactionDal(new AppConfiguration());

        public List<Vehicle> AddVehicle(string userId, Vehicle vehicle)
        {
            return vehicleDal.Create(vehicle, userId);
        }

        public byte[] UpdateImage(byte[] photo,string userId)
        {
            return userDal.UpdateImage(photo,userId);
        }
        
        public void AddAmount(float amount, string userId)
        {
            userDal.AddBalance(amount, userId);
        }

        public User FindUser(string userId)
        {
            return userDal.GetById(userId);
        }

        public bool IsUserExist(string mail)
        {
            return userDal.IsUserExist(mail);
        }

        public UserResponse SignUp(User user)
        {
           return userDal.Create(user);
        }

        public bool HasVehicle(string userId)
        {
            return vehicleDal.HasVehicle(userId);
        }

        public float GetBalance(string userId)
        {
            return userDal.GetBalance(userId);
        }

        public bool IsBalanceAvailable(float cost, string userId)
        {
            return GetBalance(userId) >= cost;
        }

        public void PayBill(string providerId, string riderId, float cost,int bookingId)
        {
            Transaction transaction = new Transaction
            {
                Amount = cost,
                From = riderId,
                To = providerId,
                BookingId=bookingId,
            };
            transactionDal.Create(transaction);
            userDal.AddBalance(cost, providerId);
            userDal.AddBalance(-cost, riderId);
        }

        

        public UserResponse Login(string password, string userId)
        {
            return userDal.Login(userId, password);
        }

        public Vehicle FindVehicle(string vehicleId)
        {
            return vehicleDal.GetById(vehicleId);
        }

        public List<Vehicle> GetVehicles(string userId)
        {
            return vehicleDal.GetVehicles(userId);
        }

        public List<Vehicle> GetAllVehicles()
        {
            return vehicleDal.GetAllVehicles();
        }

        public User UpdateUserDetails(User user)
        {
            return userDal.Update(user);
        }

        public byte[] GetImage(string userId)
        {
            return userDal.GetImage(userId);
        }

        public List<User> GetAllUsers()
        {
            return userDal.GetAllUsers();
        }
        public bool ChangePassword(UpdatePassword updatePassword,string userId)
        {
            return userDal.UpdatePassword(updatePassword,userId);
        }
        public void UpdateVehicle(Vehicle vehicle, string userId,string vehicleId)
        {
            vehicleDal.Update(vehicle, userId,vehicleId);
        }

        public List<Transaction> GetTransactions(string userId)
        {
            return transactionDal.GetTransactionsByUserId(userId);
        }
    }
}