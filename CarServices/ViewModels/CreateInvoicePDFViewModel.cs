using CarServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarServices.ViewModels
{
    public class CreateInvoicePDFViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public double SinglePrice { get; set; }
        public double SummaryPrice { get; set; }
    }
}
