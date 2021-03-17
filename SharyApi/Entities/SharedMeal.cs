using System;
using System.Collections.Generic;

#nullable disable

namespace SharyApi.Entities
{
    public partial class SharedMeal
    {
        public Guid Id { get; set; }
        public Guid StationId { get; set; }
        public DateTime ShareDateTime { get; set; }

        public virtual Station Station { get; set; }
    }
}
