﻿using SharyApi.Entities;

namespace SharyApi.Data
{
    public class DonationRepository : IDonationRepository
    {
        public DonationRepository(Shary2Context context)
        {
            Context = context;
        }

        public Shary2Context Context { get; }

        public FoodDonation CreateFoodDonation(FoodDonation donation)
        {
            var createFoodDonation = Context.Add(donation);
            return createFoodDonation.Entity;
        }

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
