using System;

namespace SharyApi.Models.Business
{
    public class BusinessUpdateDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Tin { get; set; }
        public string BankAccount { get; set; }
        public bool AcceptSolidarityMeal { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Username { get; set; }
    }
}
