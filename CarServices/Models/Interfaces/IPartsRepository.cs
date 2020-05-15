using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarServices.Models
{
    public interface IPartsRepository
    {
        Parts GetParts(int Id);
        IEnumerable<Parts> GetAllCar();
        Parts Add(Parts parts);
        Parts Update(Parts partsChanges);
        Parts Delete(int id);
    }
}
