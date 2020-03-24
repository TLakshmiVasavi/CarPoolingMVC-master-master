using System.Collections.Generic;

namespace Models.Interfaces
{
    public interface IUserService
    {
        void AddVehicle(string userId, Vehicle vehicle);

        float GetBalance(string userId);

        void SignUp(User user);

        void AddAmount(float amount, string userId);

        bool HasVehicle(string userId);

        bool IsBalanceAvailable(float cost, string userId);

        User FindUser(string userId);

        void PayBill(string providerId, string riderId, float cost);

        bool Login(string password, string userId);

        Vehicle FindVehicle(string Id);

        bool IsUserExist(string mail);

        List<string> GetVehiclesId(string id);
        
    }
}