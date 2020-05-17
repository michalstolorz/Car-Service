using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarServices.Models
{
    public class Car
    {
        public int Id { get; set; }
        [Required]
        public string VIN { get; set; }
        [Required]
        public int ProductionYear { get; set; }
        [Required]
        public int ModelId { get; set; }
        [Required]
        public int CustomerId { get; set; }
    }
}
