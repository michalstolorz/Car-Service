using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarServices.Models.Interfaces
{
    public interface ILocalDataRepository 
    {
        int GetModelId();

        void SetModelId(int Id);
    }
}
