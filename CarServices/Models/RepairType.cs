using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarServices.Models
{
    public class RepairType
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
