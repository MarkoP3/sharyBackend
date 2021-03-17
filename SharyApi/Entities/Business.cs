using System;
using System.Collections.Generic;

#nullable disable

namespace SharyApi.Entities
{
    public partial class Business
    {
        public Business()
        {
            BusinessAddresses = new HashSet<BusinessAddress>();
            FoodDonations = new HashSet<FoodDonation>();
            Payments = new HashSet<Payment>();
            SolidarityDinnerDonations = new HashSet<SolidarityDinnerDonation>();
            SolidarityMealPrices = new HashSet<SolidarityMealPrice>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Tin { get; set; }
        public string BankAccount { get; set; }
        public bool AcceptSolidarityMeal { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }

        public virtual ICollection<BusinessAddress> BusinessAddresses { get; set; }
        public virtual ICollection<FoodDonation> FoodDonations { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<SolidarityDinnerDonation> SolidarityDinnerDonations { get; set; }
        public virtual ICollection<SolidarityMealPrice> SolidarityMealPrices { get; set; }
    }
}
