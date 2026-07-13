using Cerebro.Data;
using Cerebro.Enum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cerebro.Pages.CatalogItems;

public class CreateModel : PageModel
{
    private readonly AppDbContext _context;

    public CreateModel(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult OnGet()
    {
        CatalogItem = new CatalogItem
        {
            Type = CatalogItemType.Product
        };

        return Page();
    }

    [BindProperty]
    public CatalogItem CatalogItem { get; set; } = default!;

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            if (CatalogItem.DefaultUnitPrice < 0)
            {
                throw new ArgumentException("Default unit price cannot be negative");
            }

            _context.CatalogItems.Add(CatalogItem);
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
