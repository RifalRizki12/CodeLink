using API.DTOs.Accounts;
using API.DTOs.Employees;
using API.Models;
using API.Utilities.Handler;
using CLIENT.Models;


namespace CLIENT.Contract
{
    public interface IHomeClientRepository : IRepository<Employee, Guid>
    {
        
    }
}
