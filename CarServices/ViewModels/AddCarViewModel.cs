using CarServices.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarServices.ViewModels
{
    public class AddCarViewModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength:17,MinimumLength = 17, ErrorMessage = "Wrong VIN number")]
        [RegularExpression(@"^[^IOQioq]+$", ErrorMessage = "Wrong VIN number")]
        public string VIN { get; set; }
        [Required]
        public int ProductionYear { get; set; }
        [Required]
        [Display(Name = "Car")]
        public int ChoosenModelId { get; set; }
        [Required]
        [Display(Name ="Customer")]
        public int ChoosenCustomerId { get; set; }
        public List<CarBrand> CarBrands { get; set; }
        public List<SelectListItem> CustomersList { get; set; }
        public SelectList FilteredModels { get; set; }
    }
}
