using Cerebro.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cerebro.Pages.Invoices;

public class IndexModel : PageModel
{
    private readonly AppDbContext _context;

    public IndexModel(AppDbContext context)
    {
        _context = context;
    }

    public IList<Invoice> Invoices { get; set; } = default!;

    public void OnGet()
    {
        Invoices = _context.Invoices
            .OrderByDescending(i => i.IssueDate)
            .ThenByDescending(i => i.Id)
            .ToList();
    }
}
