using Models.Interfaces;
using Models;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    
    public class UserService : IUserService
    {
        List<User> Users = new List<User>();
        public void AddVehicle(string userId, Vehicle vehicle)
        {
            User User = FindUser(userId);
            User.Vehicles.Add(vehicle);
        }
        
        public void AddAmount(float amount, string userId)
        {
            User user = FindUser(userId);
            user.Wallet.Balance += amount;
        }

        public User FindUser(string userId)
        {
            return Users.Find(u => u.Id == userId);
        }

        public bool IsUserExist(string mail)
        {
            return Users.Any(u => u.Mail == mail);
        }

        public void SignUp(User user)
        {
            user.Id = user.Mail;
            user.Wallet = new Wallet
            {
                Balance = 0
            };
            Users.Add(user);
        }

        public bool HasVehicle(string userId)
        {
            User user = FindUser(userId);
            return user.Vehicles.Count != 0;
        }

        public float ViewBalance(string userId)
        {
            User user = FindUser(userId);
            return user.Wallet.Balance;
        }

        public bool IsBalanceAvailable(float cost, string userId)
        {
            User user = FindUser(userId);
            return user.Wallet.Balance >= cost;
        }

        public void PayBill(string providerId, string riderId, float cost)
        {
            User rideProvider = FindUser(providerId);
            User rider = FindUser(riderId);
            rider.Wallet.Balance -= cost;
            rideProvider.Wallet.Balance += cost;
        }

        public bool Login(string password, string userId)
        {
            User user = FindUser(userId);
            return user.Password == password;
        }

        public Vehicle FindVehicle(string carId,string ownerId)
        {
            User provider = FindUser(ownerId);
            return provider.Vehicles.Find(c => c.Number == carId);
        }

        public List<string> GetVehiclesId(string userId)
        {
            User user = FindUser(userId);
            List<string> vehiclesId = new List<string>();
            user.Vehicles.ForEach(_ => vehiclesId.Add(_.Number));
            return vehiclesId;
        }
    }
}