using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarServices.Models
{
    public class SQLCustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext context;

        public SQLCustomerRepository(AppDbContext context)
        {
            this.context = context;
        }

        public Customer Add(Customer customer)
        {
            context.Customer.Add(customer);
            context.SaveChanges();
            return customer;
        }

        public Customer Delete(int id)
        {
            Customer customer = context.Customer.Find(id);
            if (customer != null)
            {
                context.Customer.Remove(customer);
                context.SaveChanges();
            }
            return customer;
        }

        public IEnumerable<Customer> GetAllCustomer()
        {
            return context.Customer;
        }

        public Customer GetCustomer(int Id)
        {
            return context.Customer.Find(Id);
        }

        public Customer Update(Customer customerChanges)
        {
            var customer = context.Customer.Attach(customerChanges);
            customer.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return customerChanges;
        }
    }
}
