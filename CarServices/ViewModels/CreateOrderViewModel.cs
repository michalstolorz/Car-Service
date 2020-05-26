using CarServices.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarServices.ViewModels
{
    public class CreateOrderViewModel
    {
        public int Id { get; set; }
        public List<OrderDetails> PartsToOrderList { get; set; }
        public List<Parts> AllPartsList { get; set; }
        //[Required]
        public int ChoosenPartId { get; set; }
        [Required(ErrorMessage = "Quantity is required")]
        [Display(Name = "Quantity")]
        [Range(1, 2147483647,ErrorMessage = "Quantity should be betwen 1 and 2 147 483 647")]
        public int AddedQuantity { get; set; }
    }
}
