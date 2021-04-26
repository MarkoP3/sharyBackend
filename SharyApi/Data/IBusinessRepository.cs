using LaYumba.Functional;
using SharyApi.Entities;
using System;
using System.Collections.Generic;

namespace SharyApi.Data
{
    public interface IBusinessRepository
    {
        ICollection<Business> GetAllBusinesses();
        Option<Business> GetBusinessByID(Guid ID);
        Business GetBusinessCredentialsByUsername(string username);
        void DeleteBusiness(Guid ID);
        Business CreateBusiness(Business business);
        ICollection<BusinessAddress> GetAllAddressesOfBusiness(Guid businessId);
        BusinessAddress CreateBusinessAddress(BusinessAddress address);
        ICollection<Business> GetBusinessWithSolidarityDinners(Guid cityID);
        ICollection<Business> GetAllBusinessWithSolidarityDinners();
        ICollection<City> GetCitiesOfSolidarityBusinesses(Guid countryID);
        ICollection<Country> GetCountriesSolidarityDinner();
        SolidarityMealPrice GetMealPriceOfBusiness(Guid ID);
        bool SaveChanges();
    }
}
