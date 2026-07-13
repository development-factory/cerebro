using Cerebro.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cerebro.Pages.Employees;

public class CreateModel : PageModel
{
    private readonly AppDbContext _context;

    public CreateModel(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult OnGet()
    {
        return Page();
    }

    [BindProperty]
    public Employee Employee { get; set; } = default!;

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            if (Employee.HiringDate < DateTime.Today)
            {
                throw new ArgumentException("Hiring date cannot be in the past");
            }

            if (Employee.ExitDate.HasValue)
            {
                throw new ArgumentException("Exit date cannot be set on creation");
            }

            if (Employee.Salary < 0)
            {
                throw new ArgumentException("Salary cannot be negative");
            }

            if (Employee.DateOfBirth > DateTime.Today)
            {
                throw new ArgumentException("Date of birth cannot be in the future");
            }

            _context.Employees.Add(Employee);
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

        return RedirectToPage("../Index");
    }
}
