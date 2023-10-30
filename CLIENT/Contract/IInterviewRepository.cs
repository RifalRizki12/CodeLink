using API.DTOs.Accounts;
using API.DTOs.Employees;
using API.DTOs.Interviews;
using API.Models;
using API.Utilities.Handler;
using CLIENT.Models;


namespace CLIENT.Contract
{
    public interface IInterviewRepository : IRepository<InterviewDto, Guid>
    {
       
    }
}
