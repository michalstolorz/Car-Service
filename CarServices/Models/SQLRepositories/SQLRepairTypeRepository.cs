using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarServices.Models
{
    public class SQLRepairTypeRepository : IRepairTypeRepository
    {
        private readonly AppDbContext context;

        public SQLRepairTypeRepository(AppDbContext context)
        {
            this.context = context;
        }

        public RepairType Add(RepairType repairType)
        {
            context.RepairType.Add(repairType);
            context.SaveChanges();
            return repairType;
        }

        public RepairType Delete(int id)
        {
            RepairType repairType = context.RepairType.Find(id);
            if (repairType != null)
            {
                context.RepairType.Remove(repairType);
                context.SaveChanges();
            }
            return repairType;
        }

        public IEnumerable<RepairType> GetAllRepairType()
        {
            return context.RepairType;
        }

        public RepairType GetRepairType(int Id)
        {
            return context.RepairType.Find(Id);
        }

        public RepairType Update(RepairType repairTypeChanges)
        {
            var repairType = context.RepairType.Attach(repairTypeChanges);
            repairType.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return repairTypeChanges;
        }
    }
}
