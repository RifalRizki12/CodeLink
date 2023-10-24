using API.Models;
using API.Utilities.Handler;

namespace CLIENT.Contract
{
    public interface IEmployeeRepository : IRepository<Employee, Guid>
    {
        Task<ResponseOKHandler<IEnumerable<Employee>>> GetDetailIdle();
    }
}
