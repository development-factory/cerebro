using Cerebro.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cerebro.Pages.Invoices;

public class CreateModel : PageModel
{
    private readonly AppDbContext _context;

    public CreateModel(AppDbContext context)
    {
        _context = context;
    }

    public IList<SelectListItem> CatalogItemOptions { get; set; } = [];

    [BindProperty]
    public Invoice Invoice { get; set; } = default!;

    [BindProperty]
    public List<InvoiceLineItem> LineItems { get; set; } = [];

    public IActionResult OnGet()
    {
        LoadCatalogItems();

        Invoice = new Invoice
        {
            IssueDate = DateTime.Today,
            DueDate = DateTime.Today.AddDays(30)
        };

        LineItems = CreateDefaultLineItems();

        return Page();
    }

    public IActionResult OnPost()
    {
        LoadCatalogItems();

        if (!ModelState.IsValid)
        {
            EnsureLineItems();
            return Page();
        }

        if (Invoice.DueDate < Invoice.IssueDate)
        {
            ModelState.AddModelError(string.Empty, "Due date cannot be earlier than issue date");
            EnsureLineItems();
            return Page();
        }

        var normalizedLineItems = NormalizeLineItems(LineItems);
        if (normalizedLineItems is null)
        {
            EnsureLineItems();
            return Page();
        }

        if (!CatalogItemOptions.Any())
        {
            ModelState.AddModelError(string.Empty, "Create at least one product or service before creating invoices.");
            EnsureLineItems();
            return Page();
        }

        try
        {
            var invoice = new Invoice
            {
                InvoiceNumber = Invoice.InvoiceNumber,
                ClientName = Invoice.ClientName,
                IssueDate = Invoice.IssueDate,
                DueDate = Invoice.DueDate,
                IsPaid = Invoice.IsPaid,
                Notes = Invoice.Notes,
                Amount = normalizedLineItems.Sum(line => line.LineTotal)
            };

            _context.Invoices.Add(invoice);
            _context.SaveChanges();

            foreach (var lineItem in normalizedLineItems)
            {
                lineItem.InvoiceId = invoice.Id;
                _context.InvoiceLineItems.Add(lineItem);
            }

            _context.SaveChanges();
        }
        catch (Exception)
        {
            return RedirectToPage("../Error");
        }

        return RedirectToPage("./Index");
    }

    private void LoadCatalogItems()
    {
        CatalogItemOptions = _context.CatalogItems
            .OrderBy(item => item.Type)
            .ThenBy(item => item.Name)
            .Select(item => new SelectListItem($"{item.Name} ({item.Type})", item.Id.ToString()))
            .ToList();
    }

    private List<InvoiceLineItem> CreateDefaultLineItems()
    {
        var defaultLineItem = new InvoiceLineItem { Quantity = 1 };

        if (CatalogItemOptions.Count > 0)
        {
            defaultLineItem.CatalogItemId = int.Parse(CatalogItemOptions[0].Value!);
            defaultLineItem.UnitPrice = _context.CatalogItems.Find(defaultLineItem.CatalogItemId)?.DefaultUnitPrice ?? 0m;
        }

        return [defaultLineItem];
    }

    private void EnsureLineItems()
    {
        if (LineItems.Count == 0)
        {
            LineItems = CreateDefaultLineItems();
        }
    }

    private List<InvoiceLineItem>? NormalizeLineItems(IEnumerable<InvoiceLineItem>? lineItems)
    {
        var normalized = new List<InvoiceLineItem>();
        var sourceItems = lineItems?.ToList() ?? [];

        for (var index = 0; index < sourceItems.Count; index++)
        {
            var lineItem = sourceItems[index];
            var hasAnyValue = lineItem.CatalogItemId != 0 || lineItem.Quantity != 0 || lineItem.UnitPrice != 0;

            if (!hasAnyValue)
            {
                continue;
            }

            if (lineItem.CatalogItemId == 0)
            {
                ModelState.AddModelError(string.Empty, $"Line {index + 1}: select a product or service.");
                return null;
            }

            if (lineItem.Quantity <= 0)
            {
                ModelState.AddModelError(string.Empty, $"Line {index + 1}: quantity must be at least 1.");
                return null;
            }

            if (lineItem.UnitPrice <= 0)
            {
                ModelState.AddModelError(string.Empty, $"Line {index + 1}: unit price must be greater than zero.");
                return null;
            }

            normalized.Add(new InvoiceLineItem
            {
                CatalogItemId = lineItem.CatalogItemId,
                Quantity = lineItem.Quantity,
                UnitPrice = lineItem.UnitPrice
            });
        }

        if (normalized.Count == 0)
        {
            ModelState.AddModelError(string.Empty, "Add at least one invoice line item.");
            return null;
        }

        return normalized;
    }
}
