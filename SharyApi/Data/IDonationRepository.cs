using SharyApi.Entities;

namespace SharyApi.Data
{
    public interface IDonationRepository
    {
        MoneyDonation CreateMoneyDonation(MoneyDonation moneyDonation);
        SolidarityDinnerDonation CreateSolidarityDonation(SolidarityDinnerDonation solidarityDonation);
        FoodDonation CreateFoodDonation(FoodDonation donation);
        bool SaveChanges();
    }
}
