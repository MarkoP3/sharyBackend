using System;

namespace SharyApi.Models.Business
{
    public class FoodDonationDto
    {
        public Guid Id { get; set; }
        public Guid StationId { get; set; }
        public Guid BusinessId { get; set; }
        public int Quantity { get; set; }
        public DateTime DonationDateTime { get; set; }
    }
}
