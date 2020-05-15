using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarServices.Models
{
    public interface IUsedPartsRepository
    {
        UsedParts GetUsedParts(int Id);
        IEnumerable<UsedParts> GetAllUsedParts();
        UsedParts Add(UsedParts usedParts);
        UsedParts Update(UsedParts usedPartsChanges);
        UsedParts Delete(int id);
    }
}
