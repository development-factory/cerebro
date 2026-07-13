using Cerebro.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cerebro.Pages.Invoices;

public class EditModel : PageModel
{
    private readonly AppDbContext _context;

    public EditModel(AppDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Invoice Invoice { get; set; } = default!;

    public IActionResult OnGet(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        Invoice = _context.Invoices.Find(id.Value) ?? default!;
        if (Invoice is null)
        {
            return NotFound();
        }

        return Page();
    }

    public IActionResult OnPost(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        try
        {
            var persistedInvoice = _context.Invoices.Find(id.Value);
            if (persistedInvoice is null)
            {
                return NotFound();
            }

            Invoice.Id = id.Value;

            if (Invoice.Amount < 0)
            {
                throw new ArgumentException("Amount cannot be negative");
            }

            if (Invoice.DueDate < Invoice.IssueDate)
            {
                throw new ArgumentException("Due date cannot be earlier than issue date");
            }

            persistedInvoice.InvoiceNumber = Invoice.InvoiceNumber;
            persistedInvoice.ClientName = Invoice.ClientName;
            persistedInvoice.IssueDate = Invoice.IssueDate;
            persistedInvoice.DueDate = Invoice.DueDate;
            persistedInvoice.Amount = Invoice.Amount;
            persistedInvoice.IsPaid = Invoice.IsPaid;
            persistedInvoice.Notes = Invoice.Notes;

            _context.Invoices.Update(persistedInvoice);
            _context.SaveChanges();
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
        catch (Exception)
        {
            return RedirectToPage("../Error");
        }

        return RedirectToPage("./Index");
    }
}
