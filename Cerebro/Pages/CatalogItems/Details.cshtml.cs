using Cerebro.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cerebro.Pages.CatalogItems;

public class DetailsModel : PageModel
{
    private readonly AppDbContext _context;

    public DetailsModel(AppDbContext context)
    {
        _context = context;
    }

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
}
