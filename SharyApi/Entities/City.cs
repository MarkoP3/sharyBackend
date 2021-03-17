using System;
using System.Collections.Generic;

#nullable disable

namespace SharyApi.Entities
{
    public partial class City
    {
        public City()
        {
            BusinessAddresses = new HashSet<BusinessAddress>();
            StationAddresses = new HashSet<StationAddress>();
        }

        public Guid Id { get; set; }
        public Guid CountryId { get; set; }
        public string Name { get; set; }

        public virtual Country Country { get; set; }
        public virtual ICollection<BusinessAddress> BusinessAddresses { get; set; }
        public virtual ICollection<StationAddress> StationAddresses { get; set; }
    }
}
