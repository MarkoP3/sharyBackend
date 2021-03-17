using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharyApi.Models.Station
{
    public class ReceivedMealDto
    {
        public Guid Id { get; set; }
        public Guid? StationId { get; set; }
        public int Quantity { get; set; }
        public DateTime ReceivedDateTime { get; set; }
    }
}
