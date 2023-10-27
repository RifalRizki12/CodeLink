using API.Models;

namespace API.Contracts
{
    public interface IInterviewRepository : IGeneralRepository<Interview>
    {
        Interview GetEmployeeGuid(Guid employeeGuid);
    }
}
