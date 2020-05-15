using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarServices.Models
{
    public class SQLCarRepository : ICarRepository
    {
        private readonly AppDbContext context;

        public SQLCarRepository(AppDbContext context)
        {
            this.context = context;
        }

        public Car Add(Car car)
        {
            context.Car.Add(car);
            context.SaveChanges();
            return car;
        }

        public Car Delete(int id)
        {
            Car car = context.Car.Find(id);
            if (car != null)
            {
                context.Car.Remove(car);
                context.SaveChanges();
            }
            return car;
        }

        public IEnumerable<Car> GetAllCar()
        {
            return context.Car;
        }

        public Car GetCar(int Id)
        {
            return context.Car.Find(Id);
        }

        public Car Update(Car carChanges)
        {
            var car = context.Car.Attach(carChanges);
            car.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return carChanges;
        }
    }
}
