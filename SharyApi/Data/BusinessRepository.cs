using AutoMapper;
using CSharpFunctionalExtensions;
using LaYumba.Functional;
using SharyApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using static LaYumba.Functional.F;

namespace SharyApi.Data
{
    public class BusinessRepository : IBusinessRepository
    {
        public BusinessRepository(Shary2Context context, IMapper mapper)
        {
            Context = context;
            Mapper = mapper;
        }

        public Shary2Context Context { get; }
        public IMapper Mapper { get; }

        public Business CreateBusiness(Business business)
        {
            business.Id = Guid.NewGuid();
            var createBusiness = Context.Add(business);
            return createBusiness.Entity;
        }

        public BusinessAddress CreateBusinessAddress(BusinessAddress address)
        {
            address.Id = Guid.NewGuid();
            var createBusinessAddress = Context.Add(address);
            return createBusinessAddress.Entity;
        }

        public void DeleteBusiness(Guid ID)
        {
            var business = GetBusinessByID(ID);
            Context.Remove(business);
        }

        public ICollection<BusinessAddress> GetAllAddressesOfBusiness(Guid businessId)
        {
            return Context.Businesses.Find(businessId).BusinessAddresses.ToList();
        }
        public ICollection<Business> GetAllBusinesses()
        {
            return Context.Businesses.ToList();
        }

        public ICollection<Business> GetAllBusinessWithSolidarityDinners()
        {
            return Context.Businesses.Where(b => b.AcceptSolidarityMeal).ToList();
        }

        public Option<Business> GetBusinessByID(Guid ID) => Context.Businesses.Find(ID) != null ? Some(Context.Businesses.FirstOrDefault(business => business.Id == ID)) : None;
        public Business GetBusinessCredentialsByUsername(string username)
        {
            return Context.Businesses.Where(b => b.Username == username).FirstOrDefault();
        }
        public ICollection<Business> GetBusinessWithSolidarityDinners(Guid cityID)
        {
            return Context.Businesses.Where(b => b.AcceptSolidarityMeal && b.BusinessAddresses.Where(ba => ba.CityId == cityID).ToList().Count > 0).ToList();
        }

        public ICollection<City> GetCitiesOfSolidarityBusinesses(Guid countryID)
        {
            return Context.Cities.Where(c => c.BusinessAddresses.Where(ba => ba.Business.AcceptSolidarityMeal == true).ToList().Count > 0 && c.CountryId == countryID).ToList();
        }

        public ICollection<Country> GetCountriesSolidarityDinner()
        {
            return Context.Countries.Where(c => c.Cities.Where(ci => ci.BusinessAddresses.Where(ba => ba.Business.AcceptSolidarityMeal == true).ToList().Count > 0).ToList().Count > 0).ToList();
        }

        public SolidarityMealPrice GetMealPriceOfBusiness(Guid ID)
        {
            return Context.Businesses.Find(ID).SolidarityMealPrices.Where(smp => smp.ValidTo == null).FirstOrDefault();
        }

        public bool SaveChanges()
        {
            return Context.SaveChanges() > 0;
        }

    }
}
