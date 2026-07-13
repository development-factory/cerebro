using Cerebro.Abstractions;
using Cerebro.Data;
using Cerebro.Exceptions;

namespace Cerebro.Services;

public class InvoiceService : IInvoiceService
{
    private readonly AppDbContext _context;

    public InvoiceService(AppDbContext context)
    {
        _context = context;
    }

    public void Create(Invoice invoice)
    {
        Validate(invoice);

        _context.Invoices.Add(invoice);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var invoice = _context.Invoices.Find(id)
            ?? throw new InvoiceNotFoundException("Invoice not found");

        _context.Invoices.Remove(invoice);
        _context.SaveChanges();
    }

    public IEnumerable<Invoice> GetAll()
    {
        return _context.Invoices.OrderByDescending(i => i.IssueDate).ThenByDescending(i => i.Id);
    }

    public Invoice GetById(int id)
    {
        var invoice = _context.Invoices.Find(id)
            ?? throw new InvoiceNotFoundException("Invoice not found");

        return invoice;
    }

    public void Update(Invoice invoice)
    {
        Validate(invoice);

        var persistedInvoice = _context.Invoices.Find(invoice.Id)
            ?? throw new InvoiceNotFoundException("Invoice not found");

        persistedInvoice.InvoiceNumber = invoice.InvoiceNumber;
        persistedInvoice.ClientName = invoice.ClientName;
        persistedInvoice.IssueDate = invoice.IssueDate;
        persistedInvoice.DueDate = invoice.DueDate;
        persistedInvoice.Amount = invoice.Amount;
        persistedInvoice.IsPaid = invoice.IsPaid;
        persistedInvoice.Notes = invoice.Notes;

        _context.Invoices.Update(persistedInvoice);
        _context.SaveChanges();
    }

    private static void Validate(Invoice invoice)
    {
        if (invoice.Amount < 0)
        {
            throw new ArgumentException("Amount cannot be negative");
        }

        if (invoice.DueDate < invoice.IssueDate)
        {
            throw new ArgumentException("Due date cannot be earlier than issue date");
        }
    }
}
