using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarServices.Models
{
    public interface ICarBrandRepository
    {
        CarBrand GetCarBrand(int Id);
        IEnumerable<CarBrand> GetAllCarBrand();
        CarBrand Add(CarBrand carBrand);
        CarBrand Update(CarBrand carBrandChanges);
        CarBrand Delete(int id);
    }
}
