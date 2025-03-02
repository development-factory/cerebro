using Cerebro.Data;
using Cerebro.Exceptions;

namespace Cerebro.Services;

public class EmployeeService
{
    private readonly AppDbContext _context;
    public EmployeeService(AppDbContext context)
    {
        _context = context;
    }

    public void Create(Employee employee)
    {
        if (employee.HiringDate < DateTime.Today)
        {
            throw new ArgumentException("Hiring date cannot be in the past");
        }

        if (employee.ExitDate.HasValue)
        {
            throw new ArgumentException("Exit date cannot be set on creation");
        }

        if (employee.Salary < 0)
        {
            throw new ArgumentException("Salary cannot be negative");
        }

        if (employee.DateOfBirth > DateTime.Today)
        {
            throw new ArgumentException("Date of birth cannot be in the future");
        }

        _context.Employees.Add(employee);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var employee = _context.Employees.Find(id)
            ?? throw new EmployeeNotFoundException("Employee not found");

        _context.Employees.Remove(employee);
        _context.SaveChanges();
    }

    public Employee GetById(int id)
    {
        var employee = _context.Employees.Find(id)
            ?? throw new EmployeeNotFoundException("Employee not found");

        return employee;
    }

    public IEnumerable<Employee> GetAll()
    {
        return _context.Employees;
    }

    public void Update(Employee employee)
    {
        var persistedEmployee = _context.Employees.Find(employee.Id)
            ?? throw new EmployeeNotFoundException("Employee not found");

        if (employee.Salary < 0)
        {
            throw new ArgumentException("Salary cannot be negative");
        }

        if (employee.DateOfBirth > DateTime.Today)
        {
            throw new ArgumentException("Date of birth cannot be in the future");
        }

        persistedEmployee.FirstName = employee.FirstName;
        persistedEmployee.LastName = employee.LastName;
        persistedEmployee.DateOfBirth = employee.DateOfBirth;
        persistedEmployee.HiringDate = employee.HiringDate;
        persistedEmployee.Role = employee.Role;
        persistedEmployee.Salary = employee.Salary;
        persistedEmployee.ExitDate = employee.ExitDate;

        _context.Employees.Update(persistedEmployee);
        _context.SaveChanges();
    }
}