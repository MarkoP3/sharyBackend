using SharyApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharyApi.Data
{
    public interface IDonationRepository
    {
        MoneyDonation CreateMoneyDonation(MoneyDonation moneyDonation);
        SolidarityDinnerDonation CreateSolidarityDonation(SolidarityDinnerDonation solidarityDonation);
        bool SaveChanges();
    }
}
