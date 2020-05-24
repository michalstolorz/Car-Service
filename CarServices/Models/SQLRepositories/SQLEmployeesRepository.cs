using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarServices.Models
{
    public class SQLEmployeesRepository : IEmployeesRepository
    {
        private readonly AppDbContext context;

        public SQLEmployeesRepository(AppDbContext context)
        {
            this.context = context;
        }

        public Employees Add(Employees employees)
        {
            context.Employees.Add(employees);
            context.SaveChanges();
            return employees;
        }

        public Employees Delete(int id)
        {
            Employees employees = context.Employees.Find(id);
            if (employees != null)
            {
                context.Employees.Remove(employees);
                context.SaveChanges();
            }
            return employees;
        }

        public IEnumerable<Employees> GetAllEmployees()
        {
            return context.Employees;
        }

        public Employees GetEmployees(int Id)
        {
            return context.Employees.Find(Id);
        }

        public Employees GetEmployeesByUserId(string userId)
        {
            var list = GetAllEmployees().Where(e => e.UserId == userId).ToList();
            return list.FirstOrDefault();
        }

        public Employees Update(Employees employeesChanges)
        {
            var employees = context.Employees.Attach(employeesChanges);
            employees.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return employeesChanges;
        }
    }
}
