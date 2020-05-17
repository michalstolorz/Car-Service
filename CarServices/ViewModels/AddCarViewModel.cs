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
        public string VIN { get; set; }
        [Required]
        public int ProductionYear { get; set; }
        [Required]
        public int ChoosenModelId { get; set; }
        //[Required]
        public int ChoosenCustomerId { get; set; }
        public List<CarBrand> CarBrands { get; set; }
        public SelectList FilteredModels { get; set; }
    }
}
