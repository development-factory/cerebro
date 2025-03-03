using Cerebro.Abstractions;
using Cerebro.Data;
using Cerebro.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cerebro.Pages.Employees
{
    public class DetailsModel : PageModel
    {
        private readonly IEmployeeService _employeeService;

        public DetailsModel(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        public Employee Employee { get; set; } = default!;

        public IActionResult OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                Employee = _employeeService.GetById(id.Value);
            }
            catch (EmployeeNotFoundException)
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
}
