using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CarServices.Models
{
    public class CarModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [ForeignKey("BrandId")]
        public CarBrand CarBrand { get; set; }
        public int BrandId { get; set; }
    }
}
