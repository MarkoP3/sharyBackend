using AutoMapper;
using SharyApi.Entities;
using SharyApi.Models;
using SharyApi.Models.Individual;
using SharyApi.Models.Station;
using System.Linq;

namespace SharyApi.Profiles
{
    public class StationProfile : Profile
    {
        public StationProfile()
        {

            CreateMap<Station, StationDto>()
                .ForMember(
                dest => dest.City,
                opt => opt.MapFrom(s => s.StationAddress.City.Name)
                ).ForMember(
                dest => dest.Country,
                opt => opt.MapFrom(s => s.StationAddress.City.Country.Name)
                ).ForMember(
                dest => dest.Latitude,
                opt => opt.MapFrom(s => s.StationAddress.Latitude)
                ).ForMember(
                dest => dest.Longitude,
                opt => opt.MapFrom(s => s.StationAddress.Longitude)
                ).ForMember(
                dest => dest.Street,
                opt => opt.MapFrom(s => s.StationAddress.Street)
                ).ForMember(
                dest => dest.StreetNumber,
                opt => opt.MapFrom(s => s.StationAddress.StreetNumber)
                ).ForMember(
                dest => dest.SharedMeals,
                opt => opt.MapFrom(s => s.SharedMeals.Count)
                ).ForMember(
                dest => dest.ReceivedMeals,
                opt => opt.MapFrom(s => s.ReceivedMeals.Sum(x => x.Quantity))
                ).ForMember(
                dest => dest.AwaitingMeals,
                opt => opt.MapFrom(s => s.FoodDonations.Where(x => x.ReceivedMeals.Count == 0).Sum(x => x.Quantity))
                );
            CreateMap<SharedMeal, SharedMealDto>();
            CreateMap<MealPrice, MealPriceDto>()
                .ForMember(
                dest => dest.Code,
                opt => opt.MapFrom(src => src.Currency.Code)
                );
            CreateMap<Station, Principal>();

        }
    }
}
