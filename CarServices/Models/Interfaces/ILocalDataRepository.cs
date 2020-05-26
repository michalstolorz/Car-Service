using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarServices.Models.Interfaces
{
    public interface ILocalDataRepository 
    {
        int GetModelId();

        void SetModelId(int Id);

        List<OrderDetails> GetOrderDetails();

        void AddOrderDetail(OrderDetails orderDetails);

        void DeleteOrderDetail(int id);

        //void SetOrder(Order order);

        //Order GetOrder();

        void ClearOrderDetails();

    }
}
