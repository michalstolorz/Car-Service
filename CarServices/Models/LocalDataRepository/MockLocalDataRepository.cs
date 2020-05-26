using CarServices.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarServices.Models.LocalDataRepository
{
    public class MockLocalDataRepository : ILocalDataRepository
    {
        private int _modelId;
        private List<OrderDetails> _orderDetails;
        //private Order _order;

        public MockLocalDataRepository()
        {
            _orderDetails = new List<OrderDetails>();
            //_order = null;
        }

        public void AddOrderDetail(OrderDetails orderDetails)
        {
            _orderDetails.Add(orderDetails);
        }

        public void ClearOrderDetails()
        {
            _orderDetails.Clear();
        }

        public void DeleteOrderDetail(int id)
        {
            _orderDetails.RemoveAt(id);
        }

        public int GetModelId()
        {
            return _modelId;
        }

        public List<OrderDetails> GetOrderDetails()
        {
            return _orderDetails;
        }

        //public Order GetOrder()
        //{
        //    return _order;
        //}

        public void SetModelId(int Id)
        {
            _modelId = Id;
        }

        //public void SetOrder(Order order)
        //{
        //    _order = order;
        //}
    }
}
