using API.Contracts;
using API.Data;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class InterviewRepository : GeneralRepository<Interview>, IInterviewRepository
    {
        public InterviewRepository(CodeLinkDbContext context) : base(context) { }

        public Interview GetEmployeeGuid(Guid employeeGuid)
        {
            return _context.Tests.FirstOrDefault(c => c.EmployeeGuid == employeeGuid);
        }

    }
}
