using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarServices.Models.Interfaces
{
    public interface IRepairStatusRepository
    {
        RepairStatus GetRepairStatus(int Id);
        IEnumerable<RepairStatus> GetAllRepairStatus();
        RepairStatus Add(RepairStatus repairStatus);
        RepairStatus Update(RepairStatus repairStatusChanges);
        RepairStatus Delete(int id);
    }
}
