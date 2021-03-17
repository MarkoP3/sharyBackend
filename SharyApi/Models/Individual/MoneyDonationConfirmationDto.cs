using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharyApi.Models.Individual
{
    public class MoneyDonationConfirmationDto
    {
        public Guid IndividualID { get; set; }
        public int Quantity { get; set; }
        public Guid MealPriceID { get; set; }
        public string StripeSessionID { get; set; }
    }
}
