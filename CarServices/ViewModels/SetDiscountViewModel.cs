using CarServices.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarServices.ViewModels
{
    public class SetDiscountViewModel
    {
        [Required]
        public int Id { get; set; }
        public string Name { get; set; }
        [Required]
        [Range(0, 99, ErrorMessage = "Wrong value. Discount is from 0-99%")]
        public int Discount { get; set; }
        public List<Customer> CustomerList{ get; set; }
    }
}
