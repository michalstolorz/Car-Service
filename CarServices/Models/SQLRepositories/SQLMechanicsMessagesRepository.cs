using CarServices.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarServices.Models.SQLRepositories
{
    public class SQLMechanicsMessagesRepository : IMechanicsMessagesRepository
    {
        private readonly AppDbContext context;

        public SQLMechanicsMessagesRepository(AppDbContext context)
        {
            this.context = context;
        }

        public MechanicsMessages Add(MechanicsMessages mechanicsMessages)
        {
            context.MechanicsMessages.Add(mechanicsMessages);
            context.SaveChanges();
            return mechanicsMessages;
        }

        public MechanicsMessages Delete(int id)
        {
            MechanicsMessages mechanicsMessages = context.MechanicsMessages.Find(id);
            if (mechanicsMessages != null)
            {
                context.MechanicsMessages.Remove(mechanicsMessages);
                context.SaveChanges();
            }
            return mechanicsMessages;
        }

        public IEnumerable<MechanicsMessages> GetAllMechanicsMessages()
        {
            return context.MechanicsMessages;
        }

        public MechanicsMessages GetMechanicsMessage(int Id)
        {
            return context.MechanicsMessages.Find(Id);
        }

        public MechanicsMessages Update(MechanicsMessages mechanicsMessagesChanges)
        {
            var mechanicsMessages = context.MechanicsMessages.Attach(mechanicsMessagesChanges);
            mechanicsMessages.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return mechanicsMessagesChanges;
        }
    }
}
