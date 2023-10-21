using API.Contracts;
using API.Data;
using API.Models;

namespace API.Repositories
{
    public class CompanyRepository : GeneralRepository<Company>, ICompanyRepository
    {
        public CompanyRepository(CodeLinkDbContext context) : base(context) { }
    }
}
