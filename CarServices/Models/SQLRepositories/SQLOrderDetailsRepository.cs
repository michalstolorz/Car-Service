using CarServices.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarServices.Models
{
    public class SQLOrderDetailsRepository : IOrderDetailsRepository
    {
        private readonly AppDbContext context;

        public SQLOrderDetailsRepository(AppDbContext context)
        {
            this.context = context;
        }

        public OrderDetails Add(OrderDetails orderDetails)
        {
            context.OrderDetails.Add(orderDetails);
            context.SaveChanges();
            return orderDetails;
        }

        public OrderDetails Delete(int id)
        {
            OrderDetails orderDetails = context.OrderDetails.Find(id);
            if (orderDetails != null)
            {
                context.OrderDetails.Remove(orderDetails);
                context.SaveChanges();
            }
            return orderDetails;
        }

        public IEnumerable<OrderDetails> GetAllOrderDetails()
        {
            return context.OrderDetails;
        }

        public OrderDetails GetOrderDetails(int Id)
        {
            return context.OrderDetails.Find(Id);
        }

        public OrderDetails Update(OrderDetails orderDetailsChanges)
        {
            var orderDetails = context.OrderDetails.Attach(orderDetailsChanges);
            orderDetails.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return orderDetailsChanges;
        }
    }
}
