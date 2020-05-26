using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CarServices.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string Status { get; set; }
        [ForeignKey("EmployeesId")]
        public Employees Employees { get; set; }
        public int EmployeesId { get; set; }
        public DateTime OrderTime { get; set; }
    }
}
