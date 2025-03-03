using Cerebro.Abstractions;
using Cerebro.Data;
using Microsoft.AspNetCore.Mvc;
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

        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }

        public void OnGet()
        {
            if (!string.IsNullOrWhiteSpace(SearchString))
            {
                Employees = _employeeService.SearchByName(SearchString).ToList();
                return;
            }

            Employees = _employeeService.GetAll().ToList();
        }
    }
}
