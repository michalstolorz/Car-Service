using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarServices.Models
{
    public interface IInvoiceRepository
    {
        Invoice GetInvoice(int Id);
        IEnumerable<Invoice> GetAllInvoice();
        Invoice Add(Invoice invoice);
        Invoice Update(Invoice invoiceChanges);
        Invoice Delete(int id);
    }
}
