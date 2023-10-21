using API.Contracts;
using API.Data;
using API.Models;

namespace API.Repositories
{
    public class SalaryRepository : GeneralRepository<Salary>, ISalaryRepository
    {
        public SalaryRepository(CodeLinkDbContext context) : base(context) { }
    }
}
