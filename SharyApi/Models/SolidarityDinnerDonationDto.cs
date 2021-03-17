using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharyApi.Models
{
    public class SolidarityDinnerDonationDto
    {
        public Guid Id { get; set; }
        public Guid BusinessId { get; set; }
        public string BusinessName { get; set; }
        public Guid? IndividualId { get; set; }
        public int Quantity { get; set; }
        public string Currency { get; set; }
        public double Price { get; set; }
        public DateTime DonationDateTime { get; set; }
    }
}
