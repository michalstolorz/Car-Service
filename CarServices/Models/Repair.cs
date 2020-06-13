using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CarServices.Models
{
    public class Repair
    {
        public int Id { get; set; }
        [ForeignKey("EmployeesId")]
        public Employees Employees { get; set; }
        public int? EmployeesId { get; set; }
        //[ForeignKey("TypeId")]
        //public RepairType RepairType { get; set; }
        //[Required]
        //public int? TypeId { get; set; }
        [ForeignKey("CarId")]
        public Car Car { get; set; }
        [Required]
        public int CarId { get; set; }
        [ForeignKey("InvoiceId")]
        public Invoice Invoice { get; set; }
        public int? InvoiceId { get; set; }
        public string Status { get; set; }
        public double? Cost { get; set; }
        public DateTime? DateOfCompletion { get; set; }
    }
}
