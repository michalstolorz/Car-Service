using CarServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarServices.ViewModels
{
    public class PartsAvailiablePartsViewModel
    {
        public IEnumerable<Parts> PartsList { get; set; }
    }
}
