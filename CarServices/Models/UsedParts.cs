using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CarServices.Models
{
    public class UsedParts
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        [ForeignKey("RepairId")]
        public Repair Repair { get; set; }
        public int RepairId { get; set; }
        [ForeignKey("PartId")]
        public Parts Part { get; set; }
        public int PartId { get; set; }
    }
}
