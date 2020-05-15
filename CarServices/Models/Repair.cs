using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarServices.Models
{
    public class Repair
    {
        public int Id { get; set; }
        public int EmployeesId { get; set; }
        public int TypeId { get; set; }
        public int CarId { get; set; }
        public int? InvoiceId { get; set; }
        public string Status { get; set; }
        public int Cost { get; set; }
    }
}
