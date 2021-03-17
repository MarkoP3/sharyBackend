using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharyApi.Models.Individual
{
    public class SolidarityDonationConfirmationDto
    {
        public Guid IndividualID { get; set; }
        public Guid BusinessID { get; set; }
        public int Quantity { get; set; }
        public Guid MealPriceID { get; set; }
        public string StripeSessionID { get; set; }
    }
}
