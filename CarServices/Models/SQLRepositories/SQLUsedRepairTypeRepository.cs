using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarServices.Models
{
    public class SQLUsedRepairTypeRepository : IUsedRepairTypeRepository
    {
        private readonly AppDbContext context;

        public SQLUsedRepairTypeRepository(AppDbContext context)
        {
            this.context = context;
        }

        public UsedRepairType Add(UsedRepairType usedRepairType)
        {
            context.UsedRepairType.Add(usedRepairType);
            context.SaveChanges();
            return usedRepairType;
        }

        public UsedRepairType Delete(int id)
        {
            UsedRepairType usedRepairType = context.UsedRepairType.Find(id);
            if (usedRepairType != null)
            {
                context.UsedRepairType.Remove(usedRepairType);
                context.SaveChanges();
            }
            return usedRepairType;
        }

        public IEnumerable<UsedRepairType> GetAllUsedRepairType()
        {
            return context.UsedRepairType;
        }

        public UsedRepairType GetUsedRepairType(int Id)
        {
            return context.UsedRepairType.Find(Id);
        }

        public IEnumerable<UsedRepairType> GetUsedRepairTypeByRepairId(int repairId)
        {
            return GetAllUsedRepairType().Where(u => u.RepairId == repairId).ToList();
        }

        public UsedRepairType Update(UsedRepairType usedRepairTypeChanges)
        {
            var usedRepairType = context.UsedRepairType.Attach(usedRepairTypeChanges);
            usedRepairType.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return usedRepairTypeChanges;
        }
    }
}
