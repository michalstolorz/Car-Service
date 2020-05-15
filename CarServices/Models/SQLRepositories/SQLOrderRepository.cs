using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarServices.Models
{
    public class SQLOrderRepository : IOrderRepository
    {
        private readonly AppDbContext context;

        public SQLOrderRepository(AppDbContext context)
        {
            this.context = context;
        }

        public Order Add(Order order)
        {
            context.Order.Add(order);
            context.SaveChanges();
            return order;
        }

        public Order Delete(int id)
        {
            Order order = context.Order.Find(id);
            if (order != null)
            {
                context.Order.Remove(order);
                context.SaveChanges();
            }
            return order;
        }

        public IEnumerable<Order> GetAllOrder()
        {
            return context.Order;
        }

        public Order GetOrder(int Id)
        {
            return context.Order.Find(Id);
        }

        public Order Update(Order orderChanges)
        {
            var order = context.Order.Attach(orderChanges);
            order.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return orderChanges;
        }
    }
}
