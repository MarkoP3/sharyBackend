using System;
using System.Collections.Generic;

#nullable disable

namespace SharyApi.Entities
{
    public partial class FoodDonation
    {
        public FoodDonation()
        {
            ReceivedMeals = new HashSet<ReceivedMeal>();
        }

        public Guid Id { get; set; }
        public Guid StationId { get; set; }
        public Guid BusinessId { get; set; }
        public int Quantity { get; set; }
        public DateTime DonationDateTime { get; set; }

        public virtual Business Business { get; set; }
        public virtual Station Station { get; set; }
        public virtual ICollection<ReceivedMeal> ReceivedMeals { get; set; }
    }
}
