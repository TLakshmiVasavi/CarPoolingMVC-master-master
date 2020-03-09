using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarPoolingMVC.Models
{
    public class CarVM : VehicleVM
    {
        public CarVM()
        {
            Type = VehicleTypeVM.Car;
        }
    }
}
