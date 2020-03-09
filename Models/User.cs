using Models.Enums;
using System.Collections.Generic;

namespace Models
{
    public class User
    {
        public string Name;

        public string Mail;

        public string Id;

        public string Password;

        public Wallet Wallet;

        public string Number;

        public int Age;

        public Gender Gender;

        public List<Vehicle> Vehicles;
    }
}
