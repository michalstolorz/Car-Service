using CarServices.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarServices.Models
{
    public class SQLRepairStatusRepository : IRepairStatusRepository
    {
        private readonly AppDbContext context;

        public SQLRepairStatusRepository(AppDbContext context)
        {
            this.context = context;
        }

        public RepairStatus Add(RepairStatus repairStatus)
        {
            context.RepairStatus.Add(repairStatus);
            context.SaveChanges();
            return repairStatus;
        }

        public RepairStatus Delete(int id)
        {
            RepairStatus repairStatus = context.RepairStatus.Find(id);
            if (repairStatus != null)
            {
                context.RepairStatus.Remove(repairStatus);
                context.SaveChanges();
            }
            return repairStatus;
        }

        public IEnumerable<RepairStatus> GetAllRepairStatus()
        {
            return context.RepairStatus;
        }

        public RepairStatus GetRepairStatus(int Id)
        {
            return context.RepairStatus.Find(Id);
        }

        public RepairStatus Update(RepairStatus repairStatusChanges)
        {
            var repairStatus = context.RepairStatus.Attach(repairStatusChanges);
            repairStatus.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return repairStatusChanges;
        }
    }
}
