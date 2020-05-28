using AutoMapper;
using CarPoolingMVC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Models;
using Models.Enums;
using System.IO;
using System.Linq;

namespace CarPoolingMVC
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<User, UserVM>().ForMember(dest=>dest.Vehicle,opt=>opt.MapFrom(src=>src.Vehicles[0]));
            //CreateMap<UserVM, User>().ForMember(dest => dest.Photo, opt => opt.MapFrom<photoResolver>());
            CreateMap<UserVM, User>().ForMember(dest=>dest.Vehicles,opt=>opt.Ignore());
            CreateMap<Ride, OfferRideVM>().ForMember(dest => dest.Route, opt => opt.MapFrom(src => src.Route));
            CreateMap<OfferRideVM, Ride>().ForMember(dest => dest.Route, opt => opt.MapFrom(src => src.Route)); 
            CreateMap<Vehicle, VehicleVM>();
            CreateMap<VehicleVM, Vehicle>();
            CreateMap<Booking, BookingDetailsVM>()
                .ForMember(dest => dest.RequestStatus, opt => opt.MapFrom(src => src.Response.Status.ToString()))
                .ForMember(dest => dest.Cost, opt => opt.MapFrom(src => src.Response.Cost))
                .ForMember(dest => dest.To, opt => opt.MapFrom(src => src.Request.To))
                .ForMember(dest => dest.From, opt => opt.MapFrom(src => src.Request.From))
                .ForMember(dest => dest.VehicleType, opt => opt.MapFrom(src => src.Request.VehicleType))
                .ForMember(dest => dest.NoOfSeats, opt => opt.MapFrom(src => src.Request.NoOfSeats))
                .ForMember(dest => dest.Time, opt => opt.MapFrom(src => src.Request.Time))
                .ForMember(dest => dest.RideStatus, opt => opt.MapFrom(src => src.Response.Status))
                .ForMember(dest => dest.VehicleNumber, opt => opt.MapFrom(src => src.Response.VehicleNumber));
            CreateMap<BookingDetailsVM, Booking>();
            CreateMap<VehicleTypeVM, VehicleType>();
            CreateMap<VehicleType, VehicleTypeVM>();
            CreateMap<Gender, GenderVM>();
            CreateMap<GenderVM, Gender>();
            CreateMap<RideRequest, RequestDetailsVM>()
                .ForMember(dest=>dest.From,opt=>opt.MapFrom(src=>src.From))
                .ForMember(dest=>dest.To,opt=>opt.MapFrom(src=>src.To));
            CreateMap<RequestDetailsVM,RideRequest>()
                .ForMember(dest=>dest.From,opt=>opt.MapFrom(src=>src.From))
                .ForMember(dest=>dest.To,opt=>opt.MapFrom(src=>src.To));
            CreateMap<RideRequest, RequestVM>()
                .ForMember(dest => dest.From, opt => opt.MapFrom(src => src.From))
                .ForMember(dest => dest.To, opt => opt.MapFrom(src => src.To));
            CreateMap<RequestVM, RideRequest>()
                .ForMember(dest => dest.From, opt => opt.MapFrom(src => src.From))
                .ForMember(dest => dest.To, opt => opt.MapFrom(src => src.To));
            CreateMap<OfferedRideVM, Ride>()
                .ForMember(x => x.Route, opt => opt.MapFrom(model => model));
            CreateMap<Ride, OfferedRideVM>()
                .ForMember(x => x.From, opt => opt.MapFrom(model => model.Route.From))
                .ForMember(x => x.To, opt => opt.MapFrom(model => model.Route.To));
            CreateMap<AvailableRideVM, RideDetails>();
                //.ForMember(x => x.Route, opt => opt.MapFrom(model => model));
            CreateMap<RideDetails, AvailableRideVM>();
                //.ForMember(x => x.From, opt => opt.MapFrom(model => model.Route.From))
                //.ForMember(x => x.To, opt => opt.MapFrom(model => model.Route.To));
            CreateMap<Route, RouteVM>()
                .ForMember(dest => dest.Stops, opt => opt.MapFrom(src => src.Stops));
            CreateMap<string, Stop>()
    .ConstructUsing(str => new Stop { Location = str });
            CreateMap<Route,RouteVM>().ForMember(dest => dest.Stops, opt => opt.MapFrom(so => so.Stops.Select(t=>t.Location).ToList()));
            //CreateMap<string, Stop>().ForMember(dest => dest.Location, m => m.MapFrom(src => src));
            //CreateMap<Stop, string>().ForMember(dest => dest, m => m.MapFrom(src => src.Location));// <-- important line!
            CreateMap<RouteVM, Route>()
                .ForMember(dest => dest.Stops,m => m.MapFrom(src => src.Stops));
            CreateMap<IFormFile, byte[]>().ConvertUsing<FileToByteConverter>();
            CreateMap<byte[],IFormFile>().ConvertUsing<ByteToFileConverter>();
        }
    }
   
    public class FileToByteConverter : ITypeConverter<IFormFile, byte[]>
    {
        public byte[] Convert(IFormFile source, byte[] destination, ResolutionContext context)
        {
            if(source==null)
            {
                return null;
            }
            byte[] result;
            using (var stream = new MemoryStream())
            {
                source.CopyToAsync(stream);
                result = stream.ToArray();
            }
            return result;
        }
    }
    public class ByteToFileConverter : ITypeConverter<byte[],IFormFile>
    {
        public IFormFile Convert(byte[] source, IFormFile destination, ResolutionContext context)
        {
            if (source == null)
            {
                return null;
            }
            IFormFile result;
            using (var stream = new MemoryStream(source))
            {
                result= new FormFile(stream, 0, source.Length, "name", "fileName");
            }
            return result;
        }
    }

}
