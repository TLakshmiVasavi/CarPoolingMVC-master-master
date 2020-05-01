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

        public void AddVehicle(string userId, Vehicle vehicle)
        {
            vehicleDal.Create(vehicle, userId);
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

        public void PayBill(string providerId, string riderId, float cost)
        {
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

        public User UpdateUserDetails(User user)
        {
            return userDal.Update(user);
        }

        public byte[] GetImage(string userId)
        {
            return userDal.GetImage(userId);
        }

    }
}