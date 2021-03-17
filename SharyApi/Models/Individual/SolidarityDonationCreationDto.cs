using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharyApi.Models.Individual
{
    public class SolidarityDonationCreationDto
    {
        public Guid BusinessId { get; set; }
        public Guid IndividualID { get; set; }
        public int Quantity { get; set; }
    }
}
