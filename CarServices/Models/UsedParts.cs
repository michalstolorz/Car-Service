using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarServices.Models
{
    public class UsedParts
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int RepairId { get; set; }
        public int PartId { get; set; }
    }
}
