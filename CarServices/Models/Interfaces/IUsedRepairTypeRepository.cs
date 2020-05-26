using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarServices.Models
{
    public interface IUsedRepairTypeRepository
    {
        UsedRepairType GetUsedRepairType(int Id);
        IEnumerable<UsedRepairType> GetUsedRepairTypeByRepairId(int repairId);
        IEnumerable<UsedRepairType> GetAllUsedRepairType();
        UsedRepairType Add(UsedRepairType usedRepairType);
        UsedRepairType Update(UsedRepairType usedRepairTypeChanges);
        UsedRepairType Delete(int id);
    }
}
