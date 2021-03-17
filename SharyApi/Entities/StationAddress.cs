using System;
using System.Collections.Generic;

#nullable disable

namespace SharyApi.Entities
{
    public partial class StationAddress
    {
        public StationAddress()
        {
            Stations = new HashSet<Station>();
        }

        public Guid Id { get; set; }
        public Guid CityId { get; set; }
        public string Street { get; set; }
        public string StreetNumber { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }

        public virtual City City { get; set; }
        public virtual ICollection<Station> Stations { get; set; }
    }
}
