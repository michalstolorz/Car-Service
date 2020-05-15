using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarServices.Models
{
    public class SQLCarBrandRepository : ICarBrandRepository
    {
        private readonly AppDbContext context;

        public SQLCarBrandRepository(AppDbContext context)
        {
            this.context = context;
        }

        public CarBrand Add(CarBrand carBrand)
        {
            context.CarBrand.Add(carBrand);
            context.SaveChanges();
            return carBrand;
        }

        public CarBrand Delete(int id)
        {
            CarBrand carBrand = context.CarBrand.Find(id);
            if (carBrand != null)
            {
                context.CarBrand.Remove(carBrand);
                context.SaveChanges();
            }
            return carBrand;
        }

        public IEnumerable<CarBrand> GetAllCarBrand()
        {
            return context.CarBrand;
        }

        public CarBrand GetCarBrand(int Id)
        {
            return context.CarBrand.Find(Id);
        }

        public CarBrand Update(CarBrand carBrandChanges)
        {
            var carBrand = context.CarBrand.Attach(carBrandChanges);
            carBrand.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return carBrandChanges;
        }
    }
}
