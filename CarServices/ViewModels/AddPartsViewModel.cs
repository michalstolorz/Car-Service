using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarServices.ViewModels
{
    public class AddPartsViewModel
    {
        [Required]
        [Display(Name = "Name")]
        [StringLength(maximumLength: 1000, MinimumLength = 2, ErrorMessage = "Name is too short")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Quantity")]
        public int Quantity { get; set; }
        [Required]
        [Display(Name = "Part Price")]
        public float PartPrice { get; set; }
    }
}
