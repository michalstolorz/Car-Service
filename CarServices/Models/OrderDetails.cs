using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CarServices.Models
{
    public class OrderDetails
    {
        public int Id { get; set; }
        [ForeignKey("OrderId")]
        public Order Order { get; set; }
        public int OrderId { get; set; }
        [ForeignKey("PartId")]
        public Parts Part { get; set; }
        public int PartId { get; set; }
        public int Quantity { get; set; }
    }
}
