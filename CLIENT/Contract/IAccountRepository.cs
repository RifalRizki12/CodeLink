using API.DTOs.Accounts;
using API.Models;
using API.Utilities.Handler;

namespace CLIENT.Contract
{
    public interface IAccountRepository : IRepository<Employee, Guid>
    {
       /* Task<ResponseOKHandler<IEnumerable<Account>>> GetDetailIdle();

        Task<ResponseOKHandler<Account>> RegisterIdle(RegisterIdleDto registrationDto);*/
    }
}
