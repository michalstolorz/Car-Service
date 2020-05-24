using CarServices.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarServices.ViewModels
{
    public class AddPartToRepairViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Used Parts")]
        public List<UsedParts> UsedParts { get; set; }
        [Required]
        public int ChoosenPartId { get; set; }
        [Required]
        public int UsedPartQuantity { get; set; }
        public List<Parts> AvailableParts{ get; set; }
        public int RepairId { get; set; }
    }
}
