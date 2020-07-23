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
        //public List<RepairType> RepairTypeList { get; set; }
        [Display(Name = "Repair Type")]
        public int ChoosenTypeId { get; set; }
        public List<RepairTypeEntity> RepairTypeEntities { get; set; }
    }

    public class RepairTypeEntity
    {
        [Display(Name = "Repair Type")]
        public RepairType RepairType { get; set; }
        [Display(Name = "Repair Type")]
        public bool IsSelected { get; set; }
    }
}
