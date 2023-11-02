using API.DTOs.Accounts;
using API.DTOs.Employees;
using API.DTOs.Interviews;
using API.Models;
using API.Utilities.Handler;
using CLIENT.Models;


namespace CLIENT.Contract
{
    public interface IInterviewRepository : IRepository<Interview, Guid>
    {
        Task<ResponseOKHandler<ScheduleInterviewDto>> ScheduleUpdate(Guid guid, ScheduleInterviewDto scheduleUpdate);
        Task<ResponseOKHandler<IEnumerable<InterviewDto>>> GetAllInterview();
        Task<ResponseOKHandler<IEnumerable<GetInterviewDto>>> GetByCompanyGuid(Guid guid);
        Task<ResponseOKHandler<IEnumerable<GetOnsiteDto>>> GetOnsite(Guid guid);
        Task<ResponseOKHandler<IEnumerable<GetIdleHistoryDto>>> GetIdleHistory(Guid guid);
    }
}


