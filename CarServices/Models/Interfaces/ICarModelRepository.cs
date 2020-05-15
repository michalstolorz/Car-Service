using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarServices.Models
{
    public interface ICarModelRepository
    {
        CarModel GetCarModel(int Id);
        IEnumerable<CarModel> GetAllCarModel();
        CarModel Add(CarModel carModel);
        CarModel Update(CarModel carModelChanges);
        CarModel Delete(int id);
    }
}
