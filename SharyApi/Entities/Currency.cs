using System;
using System.Collections.Generic;

#nullable disable

namespace SharyApi.Entities
{
    public partial class Currency
    {
        public Currency()
        {
            MealPrices = new HashSet<MealPrice>();
            SolidarityMealPrices = new HashSet<SolidarityMealPrice>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

        public virtual ICollection<MealPrice> MealPrices { get; set; }
        public virtual ICollection<SolidarityMealPrice> SolidarityMealPrices { get; set; }
    }
}
