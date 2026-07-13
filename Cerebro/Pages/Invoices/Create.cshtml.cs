using Cerebro.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cerebro.Pages.Invoices;

public class CreateModel : PageModel
{
    private readonly AppDbContext _context;

    public CreateModel(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult OnGet()
    {
        Invoice = new Invoice
        {
            IssueDate = DateTime.Today,
            DueDate = DateTime.Today.AddDays(30)
        };

        return Page();
    }

    [BindProperty]
    public Invoice Invoice { get; set; } = default!;

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            if (Invoice.Amount < 0)
            {
                throw new ArgumentException("Amount cannot be negative");
            }

            if (Invoice.DueDate < Invoice.IssueDate)
            {
                throw new ArgumentException("Due date cannot be earlier than issue date");
            }

            _context.Invoices.Add(Invoice);
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
