using API.Models;

namespace API.Contracts
{
    public interface IEmployeeRepository : IGeneralRepository<Employee>
    {
        Employee GetByEmployeeEmail(string employeeEmail);
        Employee GetAdminEmployee();

        public int GetCountIdle();
        public int GetCaountHired();
        public int GetCaountAdmin();
        Employee GetByGuid(Guid? employeeGuid);
    }
}
