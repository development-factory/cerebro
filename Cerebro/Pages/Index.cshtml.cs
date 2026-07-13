using Cerebro.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cerebro.Pages;

public class IndexModel : PageModel
{
    private readonly AppDbContext _context;

    public IndexModel(AppDbContext context)
    {
        _context = context;
    }

    public IList<Employee> Employees { get; set; } = default!;

    [BindProperty(SupportsGet = true)]
    public string? SearchString { get; set; }

    public void OnGet()
    {
        if (!string.IsNullOrWhiteSpace(SearchString))
        {
            var search = SearchString.ToLower();
            Employees = _context.Employees
                .Where(e => e.FirstName.ToLower().Contains(search) ||
                            e.LastName.ToLower().Contains(search))
                .ToList();
            return;
        }

        Employees = _context.Employees.ToList();
    }
}
