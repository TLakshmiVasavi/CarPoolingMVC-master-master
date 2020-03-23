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
        readonly List<User> Users = new List<User>();
        readonly UserDal userDal = new UserDal(new AppConfiguration());
        readonly VehicleDal vehicleDal = new VehicleDal(new AppConfiguration());

        public void AddVehicle(string userId, Vehicle vehicle)
        {
            //User User = FindUser(userId);
            //User.Vehicles.Add(vehicle);
 
            vehicleDal.Create(vehicle, userId);
        }
        
        public void AddAmount(float amount, string userId)
        {
            //User user = FindUser(userId);
            //user.Wallet.Balance += amount;

            userDal.AddBalance(amount, userId);
        }

        public User FindUser(string userId)
        {
            //return Users.Find(u => u.Id == userId);

            return userDal.GetById(userId);
        }

        public bool IsUserExist(string mail)
        {
            return userDal.IsUserExist(mail);
            //return Users.Any(u => u.Mail == mail);
        }

        public void SignUp(User user)
        {
           userDal.Create(user);
        }

        public bool HasVehicle(string userId)
        {
            //User user = FindUser(userId);
            //return user.Vehicles.Count != 0;

            return vehicleDal.HasVehicle(userId);
        }

        public float GetBalance(string userId)
        {
            //User user = FindUser(userId);
            //return user.Wallet.Balance;

            return userDal.GetBalance(userId);
        }

        public bool IsBalanceAvailable(float cost, string userId)
        {
            //User user = FindUser(userId);
            //return user.Wallet.Balance >= cost;

            return GetBalance(userId) >= cost;
        }

        public void PayBill(string providerId, string riderId, float cost)
        {
            //User rideProvider = FindUser(providerId);
            //User rider = FindUser(riderId);
            //rider.Wallet.Balance -= cost;
            //rideProvider.Wallet.Balance += cost;

            userDal.AddBalance(cost, providerId);
            userDal.AddBalance(-cost, riderId);
        }

        public bool Login(string password, string userId)
        {
            //User user = FindUser(userId);
            //return user.Password == password;

            return userDal.Login(userId, password);
        }

        public Vehicle FindVehicle(string vehicleId)
        {
            //User provider = FindUser(ownerId);
            //return provider.Vehicles.Find(c => c.Number == carId);

            return vehicleDal.GetById(vehicleId);
        }

        public List<string> GetVehiclesId(string userId)
        {
            return vehicleDal.GetVehiclesId(userId);
        }

    }
}