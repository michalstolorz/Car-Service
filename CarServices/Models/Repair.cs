using System;
using System.Collections.Generic;
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
        public int EmployeesId { get; set; }
        [ForeignKey("TypeId")]
        public RepairType RepairType { get; set; }
        public int TypeId { get; set; }
        [ForeignKey("CarId")]
        public Car Car { get; set; }
        public int CarId { get; set; }
        [ForeignKey("InvoiceId")]
        public Invoice Invoice { get; set; }
        public int? InvoiceId { get; set; }
        public string Status { get; set; }
        public int Cost { get; set; }
    }
}
