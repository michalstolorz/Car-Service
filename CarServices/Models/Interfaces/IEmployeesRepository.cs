using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarServices.Models
{
    public interface IEmployeesRepository
    {
        Employees GetEmployees(int Id);
        Employees GetEmployeesByUserId(string userId);
        IEnumerable<Employees> GetAllEmployees();
        Employees Add(Employees employees);
        Employees Update(Employees employeesChanges);
        Employees Delete(int id);
    }
}
