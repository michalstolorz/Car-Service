using CarServices.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarServices.ViewModels
{
    public class SetRepairCostViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Used Parts")]
        public List<UsedParts> UsedParts { get; set; }
        public int RepairId { get; set; }
        [Required]
        [Display(Name = "Cost for work")]
        public double CostForWork { get; set; }
        public double CostForParts { get; set; }
        public Repair Repair { get; set; }
    }
}
