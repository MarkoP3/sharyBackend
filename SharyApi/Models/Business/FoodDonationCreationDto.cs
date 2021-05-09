using System;

namespace SharyApi.Models.Business
{
    public class FoodDonationCreationDto
    {
        public Guid StationId { get; set; }
        public Guid BusinessId { get; set; }
        public int Quantity { get; set; }
    }
}
