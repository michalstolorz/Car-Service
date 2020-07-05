using CarServices.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarServices.ViewModels
{
    public class CreateInvoicePDFViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        [Display(Name = "Net Price")]
        public double NetPrice { get; set; }
        [Display(Name = "Net Value")]
        public double NetValue { get; set; }
        public string Tax { get; set; }
        public double TaxValue { get; set; }
        [Display(Name = "Gross Value")]
        public double SummaryPrice { get; set; }
    }
}
