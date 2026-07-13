using Cerebro.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Cerebro.Pages.Invoices;

public class EditModel : PageModel
{
    private readonly AppDbContext _context;

    public EditModel(AppDbContext context)
    {
        _context = context;
    }

    public IList<SelectListItem> CatalogItemOptions { get; set; } = [];

    [BindProperty]
    public Invoice Invoice { get; set; } = default!;

    [BindProperty]
    public List<InvoiceLineItem> LineItems { get; set; } = [];

    public IActionResult OnGet(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        LoadCatalogItems();

        Invoice = _context.Invoices.Find(id.Value) ?? default!;
        if (Invoice is null)
        {
            return NotFound();
        }

        LineItems = _context.InvoiceLineItems
            .Where(lineItem => lineItem.InvoiceId == id.Value)
            .ToList();

        EnsureLineItems();
        return Page();
    }

    public IActionResult OnPost(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

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

        try
        {
            var persistedInvoice = _context.Invoices.Find(id.Value);
            if (persistedInvoice is null)
            {
                return NotFound();
            }

            persistedInvoice.InvoiceNumber = Invoice.InvoiceNumber;
            persistedInvoice.ClientName = Invoice.ClientName;
            persistedInvoice.IssueDate = Invoice.IssueDate;
            persistedInvoice.DueDate = Invoice.DueDate;
            persistedInvoice.IsPaid = Invoice.IsPaid;
            persistedInvoice.Notes = Invoice.Notes;
            persistedInvoice.Amount = normalizedLineItems.Sum(line => line.LineTotal);

            var existingLineItems = _context.InvoiceLineItems
                .Where(lineItem => lineItem.InvoiceId == id.Value)
                .ToList();

            _context.InvoiceLineItems.RemoveRange(existingLineItems);
            _context.SaveChanges();

            foreach (var lineItem in normalizedLineItems)
            {
                lineItem.InvoiceId = id.Value;
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

    private void EnsureLineItems()
    {
        if (LineItems.Count == 0)
        {
            LineItems = [new InvoiceLineItem { Quantity = 1 }];
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
