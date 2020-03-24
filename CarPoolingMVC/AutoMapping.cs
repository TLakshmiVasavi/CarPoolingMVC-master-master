using AutoMapper;
using CarPoolingMVC.Models;
using Models;

namespace CarPoolingMVC
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<User, UserVM>().ForMember(dest=>dest.Vehicle,opt=>opt.MapFrom(src=>src.Vehicles[0]));
            CreateMap<UserVM, User>();
            CreateMap<Ride, RideVM>().ForMember(dest => dest.Route, opt => opt.MapFrom(src => src.Route));
            CreateMap<RideVM, Ride>().ForMember(dest => dest.Route, opt => opt.MapFrom(src => src.Route)); 
            CreateMap<Vehicle, VehicleVM>();
            CreateMap<VehicleVM, Vehicle>();
            CreateMap<Booking, BookingDetailsVM>();
            CreateMap<BookingDetailsVM, Booking>();
            CreateMap<Route, RouteVM>().ForMember(dest => dest.ViaPoints, opt => opt.MapFrom(src => src.ViaPoints));
            CreateMap<RouteVM, Route>().ForMember(dest => dest.ViaPoints, opt => opt.MapFrom(src => src.ViaPoints));
            CreateMap<>



        }
    }
}
