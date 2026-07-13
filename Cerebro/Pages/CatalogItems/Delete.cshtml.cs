using Cerebro.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cerebro.Pages.CatalogItems;

public class DeleteModel : PageModel
{
    private readonly AppDbContext _context;

    public DeleteModel(AppDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public CatalogItem CatalogItem { get; set; } = default!;

    public IActionResult OnGet(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        CatalogItem = _context.CatalogItems.Find(id.Value) ?? default!;
        if (CatalogItem is null)
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

        var catalogItem = _context.CatalogItems.Find(id.Value);
        if (catalogItem is null)
        {
            return NotFound();
        }

        if (_context.InvoiceLineItems.Any(lineItem => lineItem.CatalogItemId == id.Value))
        {
            ModelState.AddModelError(string.Empty, "This product or service is used on an invoice and cannot be deleted.");
            CatalogItem = catalogItem;
            return Page();
        }

        _context.CatalogItems.Remove(catalogItem);
        _context.SaveChanges();

        return RedirectToPage("./Index");
    }
}
