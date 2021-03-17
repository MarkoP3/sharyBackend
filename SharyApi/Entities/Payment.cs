using System;
using System.Collections.Generic;

#nullable disable

namespace SharyApi.Entities
{
    public partial class Payment
    {
        public Guid Id { get; set; }
        public Guid BusinessId { get; set; }
        public Guid SolidarityMealPriceId { get; set; }
        public DateTime PaymentDateTime { get; set; }
        public int MealQuantity { get; set; }

        public virtual Business Business { get; set; }
        public virtual SolidarityMealPrice SolidarityMealPrice { get; set; }
    }
}
