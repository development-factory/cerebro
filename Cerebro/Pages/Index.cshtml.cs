using Cerebro.Abstractions;
using Cerebro.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cerebro.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IEmployeeService _employeeService;

        public IndexModel(IEmployeeService employeeService)
        {
           _employeeService = employeeService;
        }

        public IList<Employee> Employees { get; set; } = default!;

        public void OnGet()
        {
            Employees = _employeeService.GetAll().ToList();
        }
    }
}
