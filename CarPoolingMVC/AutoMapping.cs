using AutoMapper;
using Models.ViewModels;
using Models;
using Models.Enums;

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
            CreateMap<Booking, BookingDetailsVM>().ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Response.Status.ToString()))
                .ForMember(dest => dest.Cost, opt => opt.MapFrom(src => src.Response.Cost))
                .ForMember(dest => dest.Drop, opt => opt.MapFrom(src => src.Request.Drop))
                .ForMember(dest => dest.PickUp, opt => opt.MapFrom(src => src.Request.PickUp))
                .ForMember(dest => dest.VehicleType, opt => opt.MapFrom(src => src.Request.VehicleType))
                .ForMember(dest => dest.NoOfSeats, opt => opt.MapFrom(src => src.Request.NoOfSeats));
            CreateMap<BookingDetailsVM, Booking>();
            CreateMap<Route, RouteVM>().ForMember(dest => dest.ViaPoints, opt => opt.MapFrom(src => src.ViaPoints));
            CreateMap<ViaPoint, ViaPointVM>();
            CreateMap<ViaPointVM, ViaPoint>();
            CreateMap<RouteVM, Route>().ForMember(dest => dest.ViaPoints, opt => opt.MapFrom(src => src.ViaPoints));
            CreateMap<VehicleTypeVM, VehicleType>();
            CreateMap<VehicleType, VehicleTypeVM>();
            CreateMap<Gender, GenderVM>();
            CreateMap<GenderVM, Gender>();
            CreateMap<Request, RequestDetailsVM>().ForMember(dest=>dest.Source,opt=>opt.MapFrom(src=>src.PickUp))
                .ForMember(dest=>dest.Destination,opt=>opt.MapFrom(src=>src.Drop));
            CreateMap<RequestDetailsVM,Request>().ForMember(dest=>dest.PickUp,opt=>opt.MapFrom(src=>src.Source))
                .ForMember(dest=>dest.Drop,opt=>opt.MapFrom(src=>src.Destination));
            CreateMap<Request, RequestVM>().ForMember(dest => dest.Source, opt => opt.MapFrom(src => src.PickUp))
                .ForMember(dest => dest.Destination, opt => opt.MapFrom(src => src.Drop));
            CreateMap<RequestVM, Request>().ForMember(dest => dest.PickUp, opt => opt.MapFrom(src => src.Source))
                .ForMember(dest => dest.Drop, opt => opt.MapFrom(src => src.Destination));
            CreateMap<Ride, OfferedRideVM>().ForMember(dest => dest.Route, opt => opt.MapFrom(src => src.Route));
            CreateMap<OfferedRideVM, Ride>().ForMember(dest => dest.Route, opt => opt.MapFrom(src => src.Route));
            CreateMap<AvailableRideVM, Ride>().ForMember(dest => dest.Route, opt => opt.MapFrom(src => src.Route));
            CreateMap<Ride, AvailableRideVM>().ForMember(dest => dest.Route, opt => opt.MapFrom(src => src.Route));
        }
    }
}
