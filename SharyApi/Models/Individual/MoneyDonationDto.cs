using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharyApi.Models.Individual
{
    public class MoneyDonationDto
    {
        public Guid Id { get; set; }
        public Guid IndividualId { get; set; }
        public Guid MealPriceId { get; set; }
        public float Price { get; set; }
        public string Currency { get; set; }
        public int Quantity { get; set; }
        public DateTime DonationDateTime { get; set; }
    }
}
