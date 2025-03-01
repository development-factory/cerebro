using Cerebro.Enum;

namespace Cerebro.Data;

public class Employee
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public DateTime HiringDate { get; set; }
    public EmployeeRole Role { get; set; }
    public decimal Salary { get; set; }
    public DateTime? ExitDate { get; set; }
}
