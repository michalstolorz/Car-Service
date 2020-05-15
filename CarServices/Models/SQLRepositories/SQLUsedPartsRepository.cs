using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarServices.Models
{
    public class SQLUsedPartsRepository : IUsedPartsRepository
    {
        private readonly AppDbContext context;

        public SQLUsedPartsRepository(AppDbContext context)
        {
            this.context = context;
        }

        public UsedParts Add(UsedParts usedParts)
        {
            context.UsedParts.Add(usedParts);
            context.SaveChanges();
            return usedParts;
        }

        public UsedParts Delete(int id)
        {
            UsedParts usedParts = context.UsedParts.Find(id);
            if (usedParts != null)
            {
                context.UsedParts.Remove(usedParts);
                context.SaveChanges();
            }
            return usedParts;
        }

        public IEnumerable<UsedParts> GetAllUsedParts()
        {
            return context.UsedParts;
        }

        public UsedParts GetUsedParts(int Id)
        {
            return context.UsedParts.Find(Id);
        }

        public UsedParts Update(UsedParts usedPartsChanges)
        {
            var usedParts = context.UsedParts.Attach(usedPartsChanges);
            usedParts.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return usedPartsChanges;
        }
    }
}
