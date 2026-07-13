using Cerebro.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cerebro.Pages.Invoices;

public class DeleteModel : PageModel
{
    private readonly AppDbContext _context;

    public DeleteModel(AppDbContext context)
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

        var invoice = _context.Invoices.Find(id.Value);
        if (invoice is null)
        {
            return NotFound();
        }

        _context.Invoices.Remove(invoice);
        _context.SaveChanges();

        return RedirectToPage("./Index");
    }
}
