using Cerebro.Data;
using Cerebro.Enum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cerebro.Pages.CatalogItems;

public class EditModel : PageModel
{
    private readonly AppDbContext _context;

    public EditModel(AppDbContext context)
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

        try
        {
            var persistedCatalogItem = _context.CatalogItems.Find(id.Value);
            if (persistedCatalogItem is null)
            {
                return NotFound();
            }

            if (CatalogItem.DefaultUnitPrice < 0)
            {
                throw new ArgumentException("Default unit price cannot be negative");
            }

            persistedCatalogItem.Name = CatalogItem.Name;
            persistedCatalogItem.Type = CatalogItem.Type;
            persistedCatalogItem.DefaultUnitPrice = CatalogItem.DefaultUnitPrice;
            persistedCatalogItem.Description = CatalogItem.Description;

            _context.CatalogItems.Update(persistedCatalogItem);
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
