using Cerebro.Abstractions;
using Cerebro.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cerebro.Pages.Employees
{
    public class CreateModel : PageModel
    {
        private readonly IEmployeeService _employeeService;

        public CreateModel(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
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
                _employeeService.Create(Employee);
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
}
