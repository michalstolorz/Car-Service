using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarServices.Models
{
    public class SQLCarModelRepository : ICarModelRepository
    {
        private readonly AppDbContext context;

        public SQLCarModelRepository(AppDbContext context)
        {
            this.context = context;
        }

        public CarModel Add(CarModel carModel)
        {
            context.CarModel.Add(carModel);
            context.SaveChanges();
            return carModel;
        }

        public CarModel Delete(int id)
        {
            CarModel carModel = context.CarModel.Find(id);
            if (carModel != null)
            {
                context.CarModel.Remove(carModel);
                context.SaveChanges();
            }
            return carModel;
        }

        public IEnumerable<CarModel> GetAllCarModel()
        {
            return context.CarModel;
        }

        public CarModel GetCarModel(int Id)
        {
            return context.CarModel.Find(Id);
        }

        public CarModel Update(CarModel carModelChanges)
        {
            var carModel = context.CarModel.Attach(carModelChanges);
            carModel.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return carModelChanges;
        }
    }
}
