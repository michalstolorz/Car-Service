using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarServices.Models.Interfaces
{
    public interface IMechanicsMessagesRepository
    {
        MechanicsMessages GetMechanicsMessage(int Id);
        IEnumerable<MechanicsMessages> GetAllMechanicsMessages();
        MechanicsMessages Add(MechanicsMessages repair);
        MechanicsMessages Update(MechanicsMessages repairChanges);
        MechanicsMessages Delete(int id);
    }
}
