using Cerebro.Abstractions;
using Cerebro.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cerebro.Pages.Invoices;

public class IndexModel : PageModel
{
    private readonly IInvoiceService _invoiceService;

    public IndexModel(IInvoiceService invoiceService)
    {
        _invoiceService = invoiceService;
    }

    public IList<Invoice> Invoices { get; set; } = default!;

    public void OnGet()
    {
        Invoices = _invoiceService.GetAll().ToList();
    }
}
