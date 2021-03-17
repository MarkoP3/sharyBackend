using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharyApi.Models.Station
{
    public class StationDto
    {
        public Guid Id { get; set; }
        public Guid StationAddressId { get; set; }
        public string Street { get; set; }
        public string StreetNumber { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public int ReceivedMeals { get; set; }
        public int SharedMeals { get; set; }
        public int AwaitingMeals { get; set; }
    }
}
