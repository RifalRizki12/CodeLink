using API.Contracts;
using API.Data;
using API.Models;

namespace API.Repositories
{
    public class TestRepository : GeneralRepository<Test>, ITestRepository
    {
        public TestRepository(CodeLinkDbContext context) : base(context) { }
    }
}
