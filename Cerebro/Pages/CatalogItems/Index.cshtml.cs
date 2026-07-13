using Cerebro.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cerebro.Pages.CatalogItems;

public class IndexModel : PageModel
{
    private readonly AppDbContext _context;

    public IndexModel(AppDbContext context)
    {
        _context = context;
    }

    public IList<CatalogItem> CatalogItems { get; set; } = default!;

    public void OnGet()
    {
        CatalogItems = _context.CatalogItems
            .OrderBy(item => item.Type)
            .ThenBy(item => item.Name)
            .ToList();
    }
}
