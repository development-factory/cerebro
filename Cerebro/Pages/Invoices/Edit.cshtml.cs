using Cerebro.Abstractions;
using Cerebro.Data;
using Cerebro.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cerebro.Pages.Invoices;

public class EditModel : PageModel
{
    private readonly IInvoiceService _invoiceService;

    public EditModel(IInvoiceService invoiceService)
    {
        _invoiceService = invoiceService;
    }

    [BindProperty]
    public Invoice Invoice { get; set; } = default!;

    public IActionResult OnGet(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        try
        {
            Invoice = _invoiceService.GetById(id.Value);
        }
        catch (InvoiceNotFoundException)
        {
            return NotFound();
        }
        catch (Exception)
        {
            return RedirectToPage("../Error");
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
            Invoice.Id = id.Value;
            _invoiceService.Update(Invoice);
        }
        catch (InvoiceNotFoundException)
        {
            return NotFound();
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
