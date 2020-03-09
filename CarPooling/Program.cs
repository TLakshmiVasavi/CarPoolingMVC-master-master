using Models;
using Models.Enums;
using Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CarPooling
{
    class Program
    {
        static IUserService userService = new Services.UserService();
        static IRideService rideService = new Services.RideService();
        
        static void Main(string[] args)
        {
            int choice;
            do
            {
                Console.WriteLine("\nPlease select your choice\n" +
                    "1.Login\n" +
                    "2.SignUp\n" +
                    "3.exit");
                choice = Reader.ReadInt(1, 3);
                switch (choice)
                {
                    case 1:
                        Console.WriteLine("Please enter your id");
                        string id = Reader.ReadString();
                        if (userService.IsUserExist(id))
                        {
                            Console.WriteLine("Sorry,The Id is not Valid");
                        }
                        else
                        {
                            Console.WriteLine("Please enter your Password");
                            string password = Reader.ReadString();
                            if (userService.Login(password, id))
                            {
                                Console.WriteLine("Logged in successfully");
                                Menu(userService.FindUser(id));
                            }
                            else
                            {
                                Console.WriteLine("Sorry ,The Password is not valid");
                            }
                        }
                        break;
                    case 2:
                        User newUser =new User();
                        Console.WriteLine("Please enter your name");
                        newUser.Name = Reader.ReadString();
                        Console.WriteLine("Please enter your Password");
                        newUser.Password = Reader.ReadPassword();
                        Console.WriteLine("Please enter your Age");
                        newUser.Age = Reader.ReadInt(1, 100);
                        Console.WriteLine("Please enter your Gender\n" +
                            "1.Male\n" +
                            "2.Female");
                        newUser.Gender = Reader.ReadGender();
                        Console.WriteLine("Please enter your Contact number");
                        newUser.Number = Reader.ReadMobileNumber();
                        Console.WriteLine("Please enter your mail");
                        newUser.Mail = Reader.ReadMail();
                        if (userService.IsUserExist(newUser.Mail))
                        {
                            Console.WriteLine("Already has an account,Please Login ");
                            break;
                        }
                        newUser.Vehicles = new List<Vehicle>();
                        do
                        {
                            Console.WriteLine("Please enter Y if you want to add a vehicle,N otherwise");
                            bool hasCar = Reader.ReadBool();
                            if (hasCar)
                            {
                                Console.WriteLine("Please select the type of vehicle" +
                                    "1.Car,2.Bike");
                                VehicleType type = Reader.ReadVehicleType();
                                newUser.Vehicles.Add(NewVehicle(type));
                            }
                            else
                            {
                                break;
                            }
                        } while (true);
                        userService.SignUp(newUser);
                        Menu(newUser);
                        break;
                    case 3:
                        break;
                }
            } while (choice != 3);
        }
        static Vehicle NewVehicle(VehicleType VehicleType)
        {
            Vehicle vehicle;
            switch (VehicleType)
            {
                case VehicleType.Car:
                    vehicle = new Car();
                    break;
                default:
                    vehicle = new Vehicle();
                    break;
            }
            Console.WriteLine("Please enter the car number");
            vehicle.Number = Reader.ReadString();
            Console.WriteLine("Please enter the car model");
            vehicle.Model = Reader.ReadString();
            if (VehicleType == VehicleType.Bike)
            {
                vehicle.Capacity = 2;
            }
            else
            {
                Console.WriteLine("Please enter the number of seats in car");
                vehicle.Capacity = Reader.ReadInt();
            }
            return vehicle;
        }

        static void Menu(User user)
        {
            //int choice;
            Choice choice;
            string pickUp;
            string drop;
            float amount;
            do
            {
                Console.WriteLine("\nPlease enter your Choice\n" +
                "1.Offer ride\n" +
                "2.Book a Car\n" +
                "3.View Offered Rides and Approve Requests\n" +
                "4.View Bookings\n" +
                "5.Add Amount to Wallet\n" +
                "6.View Balance\n" +
                "7.Add a Car\n" +
                "8.Logout");
                choice = Enum.Parse<Choice>(Reader.ReadInt(1, 8).ToString());
                switch (choice)
                {
                    case Choice.OfferRide:
                        Ride ride = new Ride();
                        if (userService.HasVehicle(user.Id))
                        {
                            if (user.Age < 18)
                            {
                                Console.WriteLine("Sorry,Minors can't offer a ride");
                            }
                            else
                            {
                                ride.ProviderId = user.Id;
                                Console.WriteLine("Please enter the date and time (dd/mm/yyyy hh:mm) of your journey ");
                                ride.StartDateTime = Reader.ReadDateTime();
                                Console.WriteLine("Please enter the starting place of your journey");
                                ride.Route.Source = Reader.ReadString().ToLower();
                                Console.WriteLine("Please enter the destination of your journey");
                                ride.Route.Destination = Reader.ReadString().ToLower();
                                Console.WriteLine("Please enter the distance in KM");
                                ride.Distance = Reader.ReadFloat();
                                ride.EndDateTime = ride.StartDateTime + rideService.CalculateTime(ride.Distance);
                                ride.Route.TotalDistance = ride.Distance;
                                if (rideService.FindOfferedRides(user.Id).Any(_ => (_.StartDateTime <= ride.StartDateTime && ride.StartDateTime < _.EndDateTime) || (_.StartDateTime < ride.EndDateTime && _.EndDateTime > ride.EndDateTime)))
                                {
                                    Console.WriteLine("Sorry,You can't Offer two rides simultaneously");
                                    break;
                                }
                                do
                                {
                                    Console.WriteLine("Please enter Y to add a stop Over point,N otherwise");
                                    bool isWilling = Reader.ReadBool();
                                    if (isWilling)
                                    {
                                        ViaPoint viaPoint = new ViaPoint();
                                        Console.WriteLine("Please enter the Address");
                                        viaPoint.Location = Reader.ReadString().ToLower();
                                        Console.WriteLine("Please enter the distance from " + ride.Route.Source);
                                        viaPoint.Distance = Reader.ReadFloat(0, ride.Distance);
                                        ride.Route.ViaPoints.Add(viaPoint);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                } while (true);
                                Console.WriteLine("Please enter the cost per unit distance");
                                ride.UnitDistanceCost = Reader.ReadFloat();
                                ride.ProviderName = user.Name;
                                int carIndex;
                                if (user.Vehicles.Count == 1)
                                {
                                    carIndex = 0;
                                }
                                else
                                {
                                    Console.WriteLine("Please select the car ");
                                    foreach (var item in user.Vehicles.Select((value, index) => new { value, index }))
                                    {
                                        Console.WriteLine(item.index + 1 + ". ");
                                        DisplayCarDetails(item.value);
                                    }
                                    carIndex = Reader.ReadInt(1, user.Vehicles.Count) - 1;
                                }
                                int numberOfSeats = user.Vehicles[carIndex].Capacity - 1;
                                Console.WriteLine("Please enter the number of seats to Offer");
                                ride.NoOfOfferedSeats = Reader.ReadInt(1, numberOfSeats);
                                ride.VehicleId = user.Vehicles[carIndex].Number;
                                rideService.CreateRide(ride);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Please Add a car to Offer Ride");
                        }
                        break;
                    case Choice.BookACar:
                        Console.WriteLine("Please enter the Starting place");
                        pickUp = Reader.ReadString().ToLower();
                        Console.WriteLine("Please enter the Destination");
                        drop = Reader.ReadString().ToLower();
                        Console.WriteLine("Please enter the date");
                        DateTime dateTime = Reader.ReadDate();
                        Console.WriteLine("Please enter the number of seats");
                        int noOfSeats = Reader.ReadInt();
                        List<Ride> AvailableRides = rideService.FindRides(pickUp, drop, dateTime,user.Id,noOfSeats,VehicleType.Car);
                        if (AvailableRides.Count == 0)
                        {
                            Console.WriteLine("Rides are not available");
                        }
                        else
                        {
                            foreach (var item in AvailableRides.Select((value, index) => new { value, index }))
                            {
                                Console.Write(item.index+1+". ");
                                Vehicle car = userService.FindVehicle(item.value.VehicleId, item.value.ProviderId);
                                DisplayAvailableRide(item.value);
                                DisplayCarDetails(car);
                                Console.WriteLine("Start Time:"+item.value.StartDateTime+rideService.CalculateTime(item.value,pickUp,drop));
                                Console.WriteLine("Cost: " + rideService.CalculateCostForRide(item.value.Id, pickUp, drop)*noOfSeats);
                            }
                            Console.WriteLine("Please enter Y to book a car,N to Go Back");
                            bool isWilling = Reader.ReadBool();
                            if (!isWilling)
                            {
                                break;
                            }
                            Console.WriteLine("Please select a ride ");
                            int rideIndex = Reader.ReadInt(1, AvailableRides.Count)-1;
                            Ride availableRide = AvailableRides[rideIndex];
                            float cost = rideService.CalculateCostForRide(availableRide.Id,pickUp,drop)*noOfSeats;
                            if (userService.IsBalanceAvailable(cost, user.Id))
                            {
                                rideService.RequestRide(user.Id, availableRide.Id,pickUp,drop,noOfSeats);
                                Console.WriteLine("The Ride is Requested Successfully\n" +
                                    "Please wait for the approval");
                            }
                            else
                            {
                                Console.WriteLine("Sorry,Your Balance is not Sufficient");
                            }
                        }
                        break;
                    case Choice.ViewOfferedRidesAndApproveRequests:
                        List<Ride> offeredRides = rideService.FindOfferedRides(user.Id);
                        if (offeredRides.Count == 0)
                        {
                            Console.WriteLine("Whoops,No rides are offered");
                        }
                        else
                        {
                            foreach (var item in offeredRides.Select((value, index) => new { value, index }))
                            {
                                Console.Write(item.index+1 + ". ");
                                DisplayRide(item.value);
                                if (item.value.IsRideCompleted)
                                {
                                    Console.WriteLine("The Ride is Completed");
                                }
                                else
                                {
                                    Console.WriteLine("The Ride is not Completed");
                                }
                                Console.WriteLine("car number: " + item.value.VehicleId);
                            }
                            Console.WriteLine("Please select the ride to view Requests");
                            int rideIndex = Reader.ReadInt(1, offeredRides.Count)-1;
                            Ride offeredRide = offeredRides[rideIndex];
                            ViewResponses(offeredRide);
                            if (offeredRide.IsRideCompleted)
                            {
                                break;
                            }
                            while (offeredRide.Requests.Count!=0)
                            {
                                ViewRequests(offeredRide);
                                Console.WriteLine("Please enter Y to Accept or Reject Request,N otherwise");
                                bool isWilling = Reader.ReadBool();
                                if (!isWilling)
                                {
                                    break;
                                }
                                Console.WriteLine("Please select Request to Accept or Reject");
                                int requestIndex = Reader.ReadInt(1, offeredRide.Requests.Count) - 1;
                                string riderId = offeredRide.Requests[requestIndex].RiderId;
                                Console.WriteLine("Please enter Y to Approve Request,N to Reject");
                                bool isApprove = Reader.ReadBool();
                                if (isApprove)
                                {
                                    amount = rideService.CalculateCostForRide(offeredRide.Id, offeredRide.Requests[requestIndex].PickUp, offeredRide.Requests[requestIndex].Drop);
                                    User rider = userService.FindUser(riderId);
                                    if (userService.IsBalanceAvailable(amount, rider.Id))
                                    {
                                        if (rideService.ApproveRequest(offeredRide.Id, offeredRide.Requests[requestIndex].RequestId, isApprove))
                                        {
                                            userService.PayBill(user.Id, rider.Id, amount);
                                            Console.WriteLine("The request is Accepted Successfully");
                                        }
                                        else
                                        {
                                            Console.WriteLine("The Request can't be Accepted");
                                        }
                                    }
                                }
                                else
                                if (rideService.ApproveRequest(offeredRide.Id, offeredRide.Requests[requestIndex].RequestId, isApprove))
                                {
                                    Console.WriteLine("The request is Rejected ");
                                }

                            }
                        }
                        break;
                    case Choice.ViewBookings:
                        ViewBookings(user.Id);
                        break;
                    case Choice.AddAmountToWallet:
                        Console.WriteLine("Please enter the amount");
                        amount = Reader.ReadFloat();
                        userService.AddAmount(amount, user.Id);
                        break;
                    case Choice.ViewBalance:
                        float balance = userService.ViewBalance(user.Id);
                        Console.WriteLine("The Available Balance is " + balance);
                        break;
                    case Choice.AddACar:
                        Console.WriteLine("Please select the type of vehicle" +
                                    "1.Car,2.Bike");
                        VehicleType type = Reader.ReadVehicleType();
                        userService.AddVehicle(user.Id,NewVehicle(type));
                        break;
                    case Choice.Logout:
                        break;
                }
            } while (choice != Choice.Logout);
        }

        static void DisplayCarDetails(Vehicle car)
        {
            Console.WriteLine("Car Number: " + car.Number);
            Console.WriteLine("Car Model: " + car.Model);
            Console.WriteLine("Number of Seats: " + car.Capacity);
        }

        static void ViewResponses(Ride ride)
        {
            foreach (var item in ride.Bookings)
            {
                User Rider = userService.FindUser(item.Request.RiderId);
                Console.WriteLine(" A Request by " + Rider.Name + " From " + item.Request.PickUp + " To " + item.Request.Drop + " for " + item.Request.NoOfSeats + " is " + item.Response.Status);
            }

        }

        static void ViewRequests(Ride ride)
        {
            if (ride.Requests.Count == 0)
            {
                Console.WriteLine("No requests ");
            }
            else
            {
                foreach (var item in ride.Requests.Select((value, index) => new { value, index }))
                {
                    User Rider = userService.FindUser(item.value.RiderId);
                    Console.WriteLine(item.index+1+". A Request by " + Rider.Name + " From " + item.value.PickUp + " To " + item.value.Drop + " for " + item.value.NoOfSeats+ " seats. ");
                }
                
            }
        }

        static void ViewBookings(string userId)
        {
            List<Ride> bookings = rideService.FindBookings(userId);
            if (bookings.Count == 0)
            {
                Console.WriteLine("No Rides are booked ");
            }
            else
            {
                foreach (Ride ride in bookings)
                {
                    DisplayRide(ride);
                    if (ride.IsRideCompleted)
                    {
                        Console.WriteLine("The Ride is Completed");
                    }
                    else
                    {
                        Console.WriteLine("The Ride is not Completed");
                    }
                    Booking booking = ride.Bookings.Find(_ => _.Request.RiderId == userId);
                    if (booking == null)
                    {
                        Console.WriteLine("Your Request is Not Yet Approved\n");
                    }
                    else
                    {
                        Console.WriteLine("From: " + booking.Request.PickUp);
                        Console.WriteLine("To: " + booking.Request.Drop);
                        Console.WriteLine("car number: " + ride.VehicleId);
                        Console.WriteLine("Provider is: " + ride.ProviderName);
                        Status requestStatus = booking.Response.Status;
                        Console.Write("Your Request is " + requestStatus + "\n");
                    }
                }
            }
        }

        static void DisplayAvailableRide(Ride ride)
        {
            DisplayRide(ride);
            Console.WriteLine("Provider is: " + ride.ProviderName);
        }
        
        static void DisplayRide(Ride ride)
        {
            DisplayRoute(ride.Route);
            Console.WriteLine("Start Time: " + ride.StartDateTime);
            Console.WriteLine("No.of Seats: " + ride.NoOfOfferedSeats);
        }

        static void DisplayRoute(Route route)
        {
            Console.Write("Route :");
            Console.Write(route.Source+" - ");
            route.ViaPoints.ForEach(l => Console.Write(l.Location+" - "));
            Console.WriteLine(route.Destination);
        }
    }
}