using CarServices.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarServices.ViewModels
{
    public class AddCarModelViewModel
    {
        public List<CarModel> CarModels { get; set; }
        [Required]
        [Display(Name = "New car model")]
        public string NewCarModel { get; set; }
        public int CarBrandId { get; set; }
    }
}
