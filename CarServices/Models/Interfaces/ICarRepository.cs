using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarServices.Models
{
    public interface ICarRepository
    {
        Car GetCar(int Id);
        IEnumerable<Car> GetAllCar();
        Car Add(Car car);
        Car Update(Car carChanges);
        Car Delete(int id);
    }
}
