using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarServices.Models
{
    public class SQLPartsRepository : IPartsRepository
    {
        private readonly AppDbContext context;

        public SQLPartsRepository(AppDbContext context)
        {
            this.context = context;
        }

        public Parts Add(Parts parts)
        {
            context.Parts.Add(parts);
            context.SaveChanges();
            return parts;
        }

        public Parts Delete(int id)
        {
            Parts parts = context.Parts.Find(id);
            if (parts != null)
            {
                context.Parts.Remove(parts);
                context.SaveChanges();
            }
            return parts;
        }

        public IEnumerable<Parts> GetAllParts()
        {
            return context.Parts;
        }

        public Parts GetParts(int Id)
        {
            return context.Parts.Find(Id);
        }

        public Parts Update(Parts partsChanges)
        {
            var parts = context.Parts.Attach(partsChanges);
            parts.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return partsChanges;
        }
    }
}
