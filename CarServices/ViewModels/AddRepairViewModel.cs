using CarServices.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarServices.ViewModels
{
    public class AddRepairViewModel
    {
        public int Id { get; set; }
        public List<SelectListItem> CarList { get; set; }
        [Display(Name = "Car")]
        public int ChoosenCarId { get; set; }
        public List<RepairType> RepairTypeList { get; set; }
        [Display(Name = "Repair Type")]
        public int ChoosenTypeId { get; set; }
    }

    public class CarName
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
