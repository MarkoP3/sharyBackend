using System;
using System.Collections.Generic;

#nullable disable

namespace SharyApi.Entities
{
    public partial class MealPrice
    {
        public MealPrice()
        {
            MoneyDonations = new HashSet<MoneyDonation>();
        }

        public Guid Id { get; set; }
        public Guid CurrencyId { get; set; }
        public double Price { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }

        public virtual Currency Currency { get; set; }
        public virtual ICollection<MoneyDonation> MoneyDonations { get; set; }
    }
}
