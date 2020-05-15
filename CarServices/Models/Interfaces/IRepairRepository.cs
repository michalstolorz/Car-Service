using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarServices.Models
{
    public interface IRepairRepository
    {
        Repair GetRepair(int Id);
        IEnumerable<Repair> GetAllRepair();
        Repair Add(Repair repair);
        Repair Update(Repair repairChanges);
        Repair Delete(int id);
    }
}
