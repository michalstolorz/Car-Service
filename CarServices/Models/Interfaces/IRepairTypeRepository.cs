using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarServices.Models
{
    public interface IRepairTypeRepository
    {
        RepairType GetRepairType(int Id);
        IEnumerable<RepairType> GetAllRepairType();
        RepairType Add(RepairType repairType);
        RepairType Update(RepairType repairTypeChanges);
        RepairType Delete(int id);
    }
}
