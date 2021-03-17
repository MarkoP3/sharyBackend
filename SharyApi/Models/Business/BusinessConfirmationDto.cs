using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharyApi.Models.Business
{
    public class BusinessConfirmationDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
    }
}
