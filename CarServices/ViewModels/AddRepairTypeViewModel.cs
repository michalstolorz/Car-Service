using CarServices.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarServices.ViewModels
{
    public class AddRepairTypeViewModel
    {
        public List<RepairType> RepairTypes { get; set; }
        [Required]
        [Display(Name = "New repair type")]
        public string NewRepairType { get; set; }
    }
}
