using System;
using System.Collections.Generic;

#nullable disable

namespace SharyApi.Entities
{
    public partial class MoneyDonation
    {
        public Guid Id { get; set; }
        public Guid IndividualId { get; set; }
        public Guid MealPriceId { get; set; }
        public int Quantity { get; set; }
        public DateTime DonationDateTime { get; set; }
        public string StripePaymentId { get; set; }

        public virtual Individual Individual { get; set; }
        public virtual MealPrice MealPrice { get; set; }
    }
}
