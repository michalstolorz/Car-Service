using CarServices.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarServices.ViewModels
{
    public class AddCarBrandViewModel
    {
        public List<CarBrand> CarBrands { get; set; }
        [Required]
        [Display(Name = "New car brand")]
        public string NewCarBrand { get; set; }
    }
}
