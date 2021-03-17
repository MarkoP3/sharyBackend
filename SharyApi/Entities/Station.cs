using System;
using System.Collections.Generic;

#nullable disable

namespace SharyApi.Entities
{
    public partial class Station
    {
        public Station()
        {
            FoodDonations = new HashSet<FoodDonation>();
            ReceivedMeals = new HashSet<ReceivedMeal>();
            SharedMeals = new HashSet<SharedMeal>();
        }

        public Guid Id { get; set; }
        public Guid StationAddressId { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }

        public virtual StationAddress StationAddress { get; set; }
        public virtual ICollection<FoodDonation> FoodDonations { get; set; }
        public virtual ICollection<ReceivedMeal> ReceivedMeals { get; set; }
        public virtual ICollection<SharedMeal> SharedMeals { get; set; }
    }
}
