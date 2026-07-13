using Cerebro.Abstractions;
using Cerebro.Data;
using Cerebro.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cerebro.Pages.Invoices;

public class DeleteModel : PageModel
{
    private readonly IInvoiceService _invoiceService;

    public DeleteModel(IInvoiceService invoiceService)
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
            _invoiceService.Delete(id.Value);
        }
        catch (InvoiceNotFoundException)
        {
            return NotFound();
        }
        catch (Exception)
        {
            return RedirectToPage("../Error");
        }

        return RedirectToPage("./Index");
    }
}
