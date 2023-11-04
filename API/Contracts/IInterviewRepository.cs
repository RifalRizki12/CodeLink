using API.Models;

namespace API.Contracts
{
    public interface IInterviewRepository : IGeneralRepository<Interview>
    {
        Interview GetEmployeeGuid(Guid employeeGuid);
        IEnumerable<Interview> GetAllByClientGuid(Guid clientGuid);
        List<Interview> GetByEmployeeGuid(Guid employeeGuid);
    }
}
