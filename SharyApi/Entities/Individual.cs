using System;
using System.Collections.Generic;

#nullable disable

namespace SharyApi.Entities
{
    public partial class Individual
    {
        public Individual()
        {
            MoneyDonations = new HashSet<MoneyDonation>();
            SolidarityDinnerDonations = new HashSet<SolidarityDinnerDonation>();
        }

        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }

        public virtual ICollection<MoneyDonation> MoneyDonations { get; set; }
        public virtual ICollection<SolidarityDinnerDonation> SolidarityDinnerDonations { get; set; }
    }
}
