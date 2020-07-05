using CarServices.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarServices.ViewModels
{
    public class CustomerCarRepairViewModel
    {
        public Customer Customer { get; set; }
        public Repair Repair { get; set; }
        public List<UsedRepairType> UsedRepairTypes { get; set; }
        [Required]
        public string Description { get; set; }
        public int RepairId { get; set; }
    }
}
