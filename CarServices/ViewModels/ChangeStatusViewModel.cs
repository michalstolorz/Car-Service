using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarServices.ViewModels
{
    public class ChangeStatusViewModel
    {
        public int Id { get; set; }
        [Required]
        public string  Status { get; set; }
    }
}
