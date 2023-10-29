using API.DTOs.Accounts;
using API.DTOs.Employees;
using API.Models;
using API.Utilities.Handler;
using System.Security.Cryptography;

namespace CLIENT.Contract
{
    public interface IEmployeeRepository : IRepository<Employee, Guid>
    {
        Task<ResponseOKHandler<IEnumerable<EmployeeDetailDto>>> GetDetailIdle();

        Task<ResponseOKHandler<Employee>> RegisterIdle(RegisterIdleDto registrationDto);
        Task<ResponseOKHandler<Company>> RegisterClient(RegisterClientDto registrationCDto);


        Task<ResponseOKHandler<UpdateIdleDto>> UpdateIdle(UpdateIdleDto employeeDto);

        Task<ResponseOKHandler<IEnumerable<ClientDetailDto>>> GetDetailClient();
       
        Task<ResponseOKHandler<UpdateClientDto>> UpdateClient(Guid guid, UpdateClientDto clientDto);
        Task<ResponseOKHandler<ClientDetailDto>> GetGuidClient(Guid guid);




    }
}
