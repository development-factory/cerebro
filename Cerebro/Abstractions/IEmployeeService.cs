using Cerebro.Data;

namespace Cerebro.Abstractions;
public interface IEmployeeService
{
    void Create(Employee employee);
    void Delete(int id);
    IEnumerable<Employee> GetAll();
    IEnumerable<Employee> SearchByName(string name);
    Employee GetById(int id);
    void Update(Employee employee);
}