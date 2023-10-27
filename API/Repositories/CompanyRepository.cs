using API.Contracts;
using API.Data;
using API.Models;

namespace API.Repositories
{
    public class CompanyRepository : GeneralRepository<Company>, ICompanyRepository
    {
        public CompanyRepository(CodeLinkDbContext context) : base(context) { }


        public int GetCaount()
        {
            int company = _context.Companies
                 .Count();

            return company;
        }

        public Company GetCompany(Guid company)
        {
            return _context.Companies
                .FirstOrDefault(e => e.EmployeeGuid == company);
        }

        public Company GetCompaniesByEmployeeGuid(Guid employeeGuid)
        {
            return _context.Companies.FirstOrDefault(c => c.EmployeeGuid == employeeGuid);
        }
    }
}
