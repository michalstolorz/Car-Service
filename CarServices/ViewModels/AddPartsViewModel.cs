using CarServices.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarServices.ViewModels
{
    public class AddPartsViewModel
    {
        public List<Parts> PartsList { get; set; }
        [Display(Name = "Parts")]
        public int choosenPartsId { get; set; }
        [Required]
        public int addedQuantity { get; set; }
        

    }
}
