using CarServices.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarServices.Models.LocalDataRepository
{
    public class MockLocalDataRepository : ILocalDataRepository
    {
        private int _modelId;

        public MockLocalDataRepository()
        {

        }

        public int GetModelId()
        {
            return _modelId;
        }

        public void SetModelId(int Id)
        {
            _modelId = Id;
        }
    }
}
