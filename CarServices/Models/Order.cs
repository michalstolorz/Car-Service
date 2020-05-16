using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarServices.Models
{
    public class Order
    {
        public int Id { get; set; }
        public double Quantity { get; set; }
        public string Status { get; set; }
        public int EmployeesId { get; set; }
        public DateTime OrderTime { get; set; }
    }
}
