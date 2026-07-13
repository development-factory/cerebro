using Cerebro.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cerebro.Pages.Employees;

public class DeleteModel : PageModel
{
    private readonly AppDbContext _context;

    public DeleteModel(AppDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Employee Employee { get; set; } = default!;

    public IActionResult OnGet(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        Employee = _context.Employees.Find(id.Value) ?? default!;
        if (Employee is null)
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

        var employee = _context.Employees.Find(id.Value);
        if (employee is null)
        {
            return NotFound();
        }

        _context.Employees.Remove(employee);
        _context.SaveChanges();

        return RedirectToPage("../Index");
    }
}
