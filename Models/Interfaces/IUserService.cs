using System.Collections.Generic;

namespace Models.Interfaces
{
    public interface IUserService
    {
        byte[] UpdateImage(byte[] photo, string userId);

        List<Vehicle> AddVehicle(string userId, Vehicle vehicle);

        float GetBalance(string userId);

        UserResponse SignUp(User user);

        void AddAmount(float amount, string userId);

        bool HasVehicle(string userId);

        bool IsBalanceAvailable(float cost, string userId);

        User FindUser(string userId);

        void PayBill(string providerId, string riderId, float cost, int requestId);

        UserResponse Login(string password, string userId);

        Vehicle FindVehicle(string Id);

        bool IsUserExist(string mail);

        List<Vehicle> GetVehicles(string userId);

        User UpdateUserDetails(User user);

        byte[] GetImage(string userId);
        List<User> GetAllUsers();

        List<Vehicle> GetAllVehicles();
        bool ChangePassword(UpdatePassword updatePassword,string userId);
        void UpdateVehicle(Vehicle vehicle, string userId,string vehicleId);
        List<Transaction> GetTransactions(string userId);
    }
}