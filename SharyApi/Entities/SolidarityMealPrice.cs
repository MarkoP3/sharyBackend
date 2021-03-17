using System;
using System.Collections.Generic;

#nullable disable

namespace SharyApi.Entities
{
    public partial class SolidarityMealPrice
    {
        public SolidarityMealPrice()
        {
            Payments = new HashSet<Payment>();
        }

        public Guid Id { get; set; }
        public Guid BusinessId { get; set; }
        public Guid CurrencyId { get; set; }
        public double Price { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }

        public virtual Business Business { get; set; }
        public virtual Currency Currency { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
