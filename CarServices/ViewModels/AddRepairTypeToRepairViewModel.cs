using CarServices.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarServices.ViewModels
{
    public class AddRepairTypeToRepairViewModel
    {
        public int RepairId { get; set; }
        public List<RepairType> RepairTypeList { get; set; }
        [Required]
        [Display(Name = "Repair Type")]
        public int ChoosenRepairTypeId { get; set; }
        public List<UsedRepairType> UsedRepairTypeList { get; set; }
    }
}
