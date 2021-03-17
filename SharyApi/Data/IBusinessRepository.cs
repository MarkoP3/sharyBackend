using SharyApi.Entities;
using SharyApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharyApi.Data
{
    public interface IBusinessRepository
    {
        ICollection<Business> GetAllBusinesses();
        Business GetBusinessByID(Guid ID);
        Business GetBusinessCredentialsByUsername(string username);
        void DeleteBusiness(Guid ID);
        Business CreateBusiness(Business business);
        ICollection<BusinessAddress> GetAllAddressesOfBusiness(Guid businessId);
        BusinessAddress CreateBusinessAddress(BusinessAddress address);
        ICollection<Business> GetBusinessWithSolidarityDinners(Guid cityID);
        ICollection<City> GetCitiesOfSolidarityBusinesses(Guid countryID);
        ICollection<Country> GetCountriesSolidarityDinner();
        SolidarityMealPrice GetMealPriceOfBusiness(Guid ID);
        bool SaveChanges();
    }
}
