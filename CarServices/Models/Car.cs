using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarServices.Models
{
    public class Car
    {
        public int Id { get; set; }
        public string VIN { get; set; }
        public int ProductionYear { get; set;}
        public int ModelId { get; set; }
        public int CustomerId { get; set; }
    }
}
