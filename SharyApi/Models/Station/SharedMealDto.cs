using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharyApi.Models.Station
{
    public class SharedMealDto
    {
        public Guid Id { get; set; }
        public Guid StationId { get; set; }
        public DateTime ShareDateTime { get; set; }

    }
}
