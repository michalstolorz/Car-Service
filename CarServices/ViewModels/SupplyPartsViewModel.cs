using CarServices.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarServices.ViewModels
{
    public class SupplyPartsViewModel
    {
        public List<Parts> PartsList { get; set; }
        [Required]
        [Display(Name = "Parts")]
        public int ChoosenPartsId { get; set; }
        [Required]
        [Display(Name = "Quantity")]
        public int AddedQuantity { get; set; }
    }
}
