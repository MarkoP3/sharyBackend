using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharyApi.Models.Business
{
    public class BusinessDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Tin { get; set; }
        public string BankAccount { get; set; }
        public bool AcceptSolidarityMeal { get; set; }
        public int SharedSolidarityMeals { get; set; }
        public int ReceivedSolidarityMeals { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Username { get; set; }
        public double MealPrice { get; set; }
        public string Currency { get; set; }
    }
}
