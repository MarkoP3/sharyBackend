using System;
using System.Collections.Generic;

#nullable disable

namespace SharyApi.Entities
{
    public partial class SolidarityDinnerDonation
    {
        public Guid Id { get; set; }
        public Guid BusinessId { get; set; }
        public Guid? IndividualId { get; set; }
        public int Quantity { get; set; }
        public DateTime DonationDateTime { get; set; }
        public string StripePaymentId { get; set; }
        public virtual Business Business { get; set; }
        public virtual Individual Individual { get; set; }
    }
}
