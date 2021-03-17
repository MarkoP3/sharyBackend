using AutoMapper;
using SharyApi.Entities;
using SharyApi.Models;
using SharyApi.Models.Individual;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharyApi.Profiles
{
    public class IndividualProfile: Profile
    {
        public IndividualProfile()
        {
            CreateMap<Individual, IndividualDto>();
            CreateMap<Individual, Principal>();
            CreateMap<IndividualCreationDto, Individual>();
            CreateMap<IndividualUpdateDto, Individual>();
            CreateMap<Individual, IndividualConfirmationDto>();

            CreateMap<MoneyDonation, MoneyDonationDto>()
                .ForMember(
                dest=>dest.Price,
                opt=>opt.MapFrom(src=>src.MealPrice.Price)
                )
                .ForMember(
                dest=>dest.Currency,
                opt=>opt.MapFrom(src=>src.MealPrice.Currency.Code)
                );
            CreateMap<SolidarityDinnerDonation, SolidarityDinnerDonationDto>()
                .ForMember(
                dest => dest.Price,
                opt => opt.MapFrom(src => src.Business.SolidarityMealPrices.Where(p => p.ValidTo == null).FirstOrDefault().Price)
                )
                .ForMember(
                dest => dest.Currency,
                opt => opt.MapFrom(src => src.Business.SolidarityMealPrices.Where(p => p.ValidTo == null).FirstOrDefault().Currency.Code)
                )
                .ForMember(
                dest => dest.BusinessName,
                opt => opt.MapFrom(src => src.Business.Name)
                ) ;
            CreateMap<MoneyDonationConfirmationDto, MoneyDonation>();
            CreateMap<SolidarityDonationConfirmationDto, SolidarityDinnerDonation>();
        }

    }
}
