using System;

namespace SharyApi.Models.Station
{
    public class StationCreationDto
    {
        public Guid StationAddressId { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
