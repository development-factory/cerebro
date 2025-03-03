using Cerebro.Enum;
using System.ComponentModel.DataAnnotations;

namespace Cerebro.Data;

public class Employee
{
    public int Id { get; set; }

    [Display(Name = "First Name")]
    public string FirstName { get; set; }

    [Display(Name = "Last Name")]
    public string LastName { get; set; }

    [Display(Name = "Date of Birth")]
    public DateTime DateOfBirth { get; set; }

    [Display(Name = "Hiring Date")]
    public DateTime HiringDate { get; set; }
    public EmployeeRole Role { get; set; }

    [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = false)]
    public decimal Salary { get; set; }

    [Display(Name = "Exit Date")]
    public DateTime? ExitDate { get; set; }
}
