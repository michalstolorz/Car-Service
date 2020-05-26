using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CarServices.Models
{
    public class UsedRepairType
    {
        public int Id { get; set; }
        [ForeignKey("RepairId")]
        public Repair Repair{ get; set; }
        public int RepairId { get; set; }
        [ForeignKey("RepairTypeId")]
        public RepairType RepairType { get; set; }
        public int RepairTypeId { get; set; }
    }
}
