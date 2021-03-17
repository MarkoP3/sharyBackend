using SharyApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharyApi.Data
{
    public class DonationRepository : IDonationRepository
    {
        public DonationRepository(Shary2Context context)
        {
            Context = context;
        }

        public Shary2Context Context { get; }

        public MoneyDonation CreateMoneyDonation(MoneyDonation moneyDonation)
        {
            var createMoneyDonation = Context.Add(moneyDonation);
            return createMoneyDonation.Entity;
        }

        public SolidarityDinnerDonation CreateSolidarityDonation(SolidarityDinnerDonation solidarityDonation)
        {
            var createSolidarityDonation = Context.Add(solidarityDonation);
            return createSolidarityDonation.Entity;
        }

        public bool SaveChanges()
        {
            return Context.SaveChanges() > 0;
        }
    }
}
