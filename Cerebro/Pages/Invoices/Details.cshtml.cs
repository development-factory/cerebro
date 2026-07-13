using Cerebro.Abstractions;
using Cerebro.Data;
using Cerebro.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cerebro.Pages.Invoices;

public class DetailsModel : PageModel
{
    private readonly IInvoiceService _invoiceService;

    public DetailsModel(IInvoiceService invoiceService)
    {
        _invoiceService = invoiceService;
    }

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
}
