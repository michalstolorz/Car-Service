using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CarServices.Models
{
    public class Car
    {
        public int Id { get; set; }
        [Required]
        [StringLength(17, ErrorMessage = "Wrong VIN number")]
        public string VIN { get; set; }
        [Required]
        public int ProductionYear { get; set; }
        [ForeignKey("ModelId")]
        public CarModel CarModel { get; set; }
        [Required]
        public int ModelId { get; set; }
        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }
        [Required]
        public int CustomerId { get; set; }
    }
}
