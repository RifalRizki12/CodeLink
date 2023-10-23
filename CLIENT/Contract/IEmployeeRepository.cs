using API.Models;

namespace CLIENT.Contract
{
    public interface IEmployeeRepository : IRepository<Employee, Guid>
    {
    }
}
