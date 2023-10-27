using API.DTOs.Accounts;
using API.DTOs.Employees;
using API.Models;
using API.Utilities.Handler;

namespace CLIENT.Contract
{
    public interface IEmployeeRepository : IRepository<Employee, Guid>
    {
        Task<ResponseOKHandler<IEnumerable<EmployeeDetailDto>>> GetDetailIdle();

        Task<ResponseOKHandler<Employee>> RegisterIdle(RegisterIdleDto registrationDto);

        Task<ResponseOKHandler<UpdateIdleDto>> UpdateIdle(UpdateIdleDto employeeDto);

        Task<ResponseOKHandler<IEnumerable<ClientDetailDto>>> GetDetailClient();
        Task<ResponseOKHandler<UpdateClientDto>> UpdateClient(UpdateClientDto clientDto);


    }
}
