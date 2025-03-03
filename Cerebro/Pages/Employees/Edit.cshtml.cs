using Cerebro.Abstractions;
using Cerebro.Data;
using Cerebro.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cerebro.Pages.Employees
{
    public class EditModel : PageModel
    {
        private readonly IEmployeeService _employeeService;

        public EditModel(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [BindProperty]
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

        public IActionResult OnPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                _employeeService.Update(Employee);
            }   
            catch (EmployeeNotFoundException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                return RedirectToPage("../Error");
            }

            return RedirectToPage("../Index");
        }
    }
}
