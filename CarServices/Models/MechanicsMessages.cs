using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CarServices.Models
{
    public class MechanicsMessages
    {
        public int Id { get; set; }
        [Required]
        public string Message { get; set; }
        public DateTime MessageDateTime { get; set; }
        [ForeignKey("EmployeeId")]
        public Employees Employee { get; set; }
        public int EmployeeId { get; set; }
    }
}
