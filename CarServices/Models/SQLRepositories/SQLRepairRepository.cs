using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarServices.Models
{
    public class SQLRepairRepository : IRepairRepository
    {
        private readonly AppDbContext context;

        public SQLRepairRepository(AppDbContext context)
        {
            this.context = context;
        }

        public Repair Add(Repair repair)
        {
            context.Repair.Add(repair);
            context.SaveChanges();
            return repair;
        }

        public Repair Delete(int id)
        {
            Repair repair = context.Repair.Find(id);
            if (repair != null)
            {
                context.Repair.Remove(repair);
                context.SaveChanges();
            }
            return repair;
        }

        public IEnumerable<Repair> GetAllRepair()
        {
            return context.Repair;
        }

        public Repair GetRepair(int Id)
        {
            return context.Repair.Find(Id);
        }

        public Repair Update(Repair repairChanges)
        {
            var repair = context.Repair.Attach(repairChanges);
            repair.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return repairChanges;
        }
    }
}
