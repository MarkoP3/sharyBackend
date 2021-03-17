using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharyApi.Models.Individual
{
    public class MealPriceDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public double Price { get; set; }
    }
}
