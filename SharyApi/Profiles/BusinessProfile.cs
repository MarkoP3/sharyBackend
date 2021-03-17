using AutoMapper;
using SharyApi.Entities;
using SharyApi.Models;
using SharyApi.Models.Business;
using SharyApi.Models.Individual;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace SharyApi.Profiles
{
    public class BusinessProfile : Profile
    {
        public BusinessProfile()
        {
            CreateMap<Business, BusinessDto>()
                .ForMember(
                dest => dest.ReceivedSolidarityMeals,
                opt => opt.MapFrom(src => src.SolidarityDinnerDonations.Sum(x => x.Quantity))
                )
                .ForMember(
                dest=>dest.SharedSolidarityMeals,
                opt=>opt.MapFrom(src=>src.BusinessAddresses.Sum(x=>x.SharedSolidarityMeals.Count))
                )
                .ForMember(
                dest=>dest.MealPrice,
                opt=>opt.MapFrom(src=>src.SolidarityMealPrices.Where(mp=>mp.ValidTo==null).FirstOrDefault().Price)
                )
                .ForMember(
                dest => dest.Currency,
                opt => opt.MapFrom(src => src.SolidarityMealPrices.Where(mp => mp.ValidTo == null).FirstOrDefault().Currency.Code)
                );
            CreateMap<Business, Principal>();
            CreateMap<BusinessCreationDto, Business>();
            CreateMap<BusinessUpdateDto, Business>();
            CreateMap<Business, BusinessConfirmationDto>();
            CreateMap<Country, CountryDto>();
            CreateMap<City, CityDto>()
                .ForMember(
                dest=>dest.Country,
                opt=>opt.MapFrom(src=>src.Country.Name)
                );
        }
    }
}
