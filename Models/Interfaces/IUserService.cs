﻿using System.Collections.Generic;

namespace Models.Interfaces
{
    public interface IUserService
    {
        void AddVehicle(string userId, Vehicle vehicle);

        float GetBalance(string userId);

        UserResponse SignUp(User user);

        void AddAmount(float amount, string userId);

        bool HasVehicle(string userId);

        bool IsBalanceAvailable(float cost, string userId);

        User FindUser(string userId);

        void PayBill(string providerId, string riderId, float cost);

        UserResponse Login(string password, string userId);

        Vehicle FindVehicle(string Id);

        bool IsUserExist(string mail);

        List<Vehicle> GetVehicles(string userId);

        User UpdateUserDetails(User user);

        byte[] GetImage(string userId);

    }
}