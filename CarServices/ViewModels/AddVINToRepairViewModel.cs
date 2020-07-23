using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarServices.ViewModels
{
    public class AddVINToRepairViewModel
    {
        public int RepairId { get; set; }
        [Required]
        [StringLength(maximumLength: 17, MinimumLength = 17, ErrorMessage = "Wrong VIN number")]
        [RegularExpression(@"^[^IOQioq]+$", ErrorMessage = "Wrong VIN number")]
        public string VIN { get; set; }
    }
}
