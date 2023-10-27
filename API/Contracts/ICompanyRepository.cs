using API.Models;

namespace API.Contracts
{
    public interface ICompanyRepository : IGeneralRepository<Company>
    {

        int GetCaount();
        Company GetCompany(Guid company);

    }
}
