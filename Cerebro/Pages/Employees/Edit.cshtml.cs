using Cerebro.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cerebro.Pages.Employees;

public class EditModel : PageModel
{
    private readonly AppDbContext _context;

    public EditModel(AppDbContext context)
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

        try
        {
            var persistedEmployee = _context.Employees.Find(id.Value);
            if (persistedEmployee is null)
            {
                return NotFound();
            }

            if (Employee.Salary < 0)
            {
                throw new ArgumentException("Salary cannot be negative");
            }

            if (Employee.DateOfBirth > DateTime.Today)
            {
                throw new ArgumentException("Date of birth cannot be in the future");
            }

            persistedEmployee.FirstName = Employee.FirstName;
            persistedEmployee.LastName = Employee.LastName;
            persistedEmployee.DateOfBirth = Employee.DateOfBirth;
            persistedEmployee.HiringDate = Employee.HiringDate;
            persistedEmployee.Role = Employee.Role;
            persistedEmployee.Salary = Employee.Salary;
            persistedEmployee.ExitDate = Employee.ExitDate;

            _context.Employees.Update(persistedEmployee);
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
