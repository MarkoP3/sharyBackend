using System;
using System.Collections.Generic;

#nullable disable

namespace SharyApi.Entities
{
    public partial class ReceivedMeal
    {
        public Guid Id { get; set; }
        public Guid? StationId { get; set; }
        public int Quantity { get; set; }
        public DateTime ReceivedDateTime { get; set; }
        public Guid? FoodDonationId { get; set; }

        public virtual FoodDonation FoodDonation { get; set; }
        public virtual Station Station { get; set; }
    }
}
