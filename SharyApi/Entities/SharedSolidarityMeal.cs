using System;
using System.Collections.Generic;

#nullable disable

namespace SharyApi.Entities
{
    public partial class SharedSolidarityMeal
    {
        public Guid Id { get; set; }
        public Guid BusinessAddressId { get; set; }
        public DateTime ShareDateTime { get; set; }

        public virtual BusinessAddress BusinessAddress { get; set; }
    }
}
