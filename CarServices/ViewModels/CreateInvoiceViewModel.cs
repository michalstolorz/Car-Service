using CarServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarServices.ViewModels
{
    public class CreateInvoiceViewModel
    {
        public List<Repair> repairs { get; set; }
        public List<UsedRepairType> usedRepairTypes { get; set; }
    }
}
