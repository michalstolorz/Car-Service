using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarServices.Models
{
    public class SQLInvoiceRepository : IInvoiceRepository
    {
        private readonly AppDbContext context;

        public SQLInvoiceRepository(AppDbContext context)
        {
            this.context = context;
        }

        public Invoice Add(Invoice invoice)
        {
            context.Invoice.Add(invoice);
            context.SaveChanges();
            return invoice;
        }

        public Invoice Delete(int id)
        {
            Invoice invoice = context.Invoice.Find(id);
            if (invoice != null)
            {
                context.Invoice.Remove(invoice);
                context.SaveChanges();
            }
            return invoice;
        }

        public IEnumerable<Invoice> GetAllInvoice()
        {
            return context.Invoice;
        }

        public Invoice GetInvoice(int Id)
        {
            return context.Invoice.Find(Id);
        }

        public Invoice Update(Invoice invoiceChanges)
        {
            var invoice = context.Invoice.Attach(invoiceChanges);
            invoice.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return invoiceChanges;
        }
    }
}
