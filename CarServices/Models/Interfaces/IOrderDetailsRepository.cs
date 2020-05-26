using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarServices.Models.Interfaces
{
    public interface IOrderDetailsRepository
    {
        OrderDetails GetOrderDetails(int Id);
        IEnumerable<OrderDetails> GetAllOrderDetails();
        OrderDetails Add(OrderDetails orderDetails);
        OrderDetails Update(OrderDetails orderDetailsChanges);
        OrderDetails Delete(int id);
    }
}
