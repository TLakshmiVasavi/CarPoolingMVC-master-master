using CarPooling.Models.ViewModels.CustomValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;

namespace Models.ViewModels
{
    public class RideVM
    {
        public int Id { get; set; }

        public RouteVM Route { get; set; }

        [DisplayName("Start Time ")]
        [CustomDate]
        public DateTime StartDateTime { get; set; }

        [DisplayName("Number of Seats to Offer")]
        [Remote(action: "IsSeatsAvailable", controller: "User", AdditionalFields = nameof(VehicleId), ErrorMessage = "Please enter a Valid Number")]
        public int NoOfOfferedSeats { get; set; }

        [DisplayName("Distance in KM")]
        public float Distance { get; set; }

        [DisplayName("Cost for Unit Distance")]
        public float UnitDistanceCost { get; set; }

        [DefaultValue(false)]
        public bool HasViaPoints { get; set; }

        [DisplayName("Number of ViaPoints")]
        public int NoOfViaPoints { get; set; }

        public string VehicleId { get; set; }

        public List<string> AvailableVehicles { get; set; }
    }
}
