using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarServices.Models
{
    public interface IOrderRepository
    {
        Order GetOrder(int Id);
        IEnumerable<Order> GetAllOrder();
        Order Add(Order order);
        Order Update(Order orderChanges);
        Order Delete(int id);
    }
}
