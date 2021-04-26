using System;

namespace SharyApi.Models.Business
{
    public class SolidarityMealPriceDto
    {
        public Guid Id { get; set; }
        public Guid BusinessId { get; set; }
        public Guid CurrencyId { get; set; }
        public double Price { get; set; }
    }
}
