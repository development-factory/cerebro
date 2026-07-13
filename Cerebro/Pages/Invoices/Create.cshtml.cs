using Cerebro.Abstractions;
using Cerebro.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cerebro.Pages.Invoices;

public class CreateModel : PageModel
{
    private readonly IInvoiceService _invoiceService;

    public CreateModel(IInvoiceService invoiceService)
    {
        _invoiceService = invoiceService;
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
            _invoiceService.Create(Invoice);
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
