using System;
using System.Collections.Generic;

#nullable disable

namespace SharyApi.Entities
{
    public partial class BusinessAddress
    {
        public BusinessAddress()
        {
            SharedSolidarityMeals = new HashSet<SharedSolidarityMeal>();
        }

        public Guid Id { get; set; }
        public Guid CityId { get; set; }
        public Guid BusinessId { get; set; }
        public string Street { get; set; }
        public string StreetNumber { get; set; }

        public virtual Business Business { get; set; }
        public virtual City City { get; set; }
        public virtual ICollection<SharedSolidarityMeal> SharedSolidarityMeals { get; set; }
    }
}
