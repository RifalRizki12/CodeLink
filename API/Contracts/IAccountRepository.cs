using API.Models;

namespace API.Contracts
{
    public interface IAccountRepository : IGeneralRepository<Account>
    {
        Account GetByEmployeeEmail(string employeeEmail);
        int GetCaountRequested();
        int GetCaountApproved();
        int GetCaountRejected();
        int GetCaountCanceled();
        int GetCaountNonAktif();
    }
}
