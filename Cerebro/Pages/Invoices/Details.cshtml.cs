using Cerebro.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cerebro.Pages.Invoices;

public class DetailsModel : PageModel
{
    private readonly AppDbContext _context;

    public DetailsModel(AppDbContext context)
    {
        _context = context;
    }

    public Invoice Invoice { get; set; } = default!;
    public IList<InvoiceLineItem> LineItems { get; set; } = [];

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

        LineItems = _context.InvoiceLineItems
            .Where(lineItem => lineItem.InvoiceId == id.Value)
            .Select(lineItem => new InvoiceLineItem
            {
                Id = lineItem.Id,
                InvoiceId = lineItem.InvoiceId,
                CatalogItemId = lineItem.CatalogItemId,
                CatalogItem = _context.CatalogItems.FirstOrDefault(x => x.Id == lineItem.CatalogItemId),
                Quantity = lineItem.Quantity,
                UnitPrice = lineItem.UnitPrice
            })
            .ToList();

        return Page();
    }
}
