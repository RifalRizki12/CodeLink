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

        public IEnumerable<Interview> GetAllByClientGuid(Guid clientGuid)
        {
            return _context.Tests.Where(interview => interview.OwnerGuid == clientGuid).ToList();
        }

        //untuk mendapatkan list guid interview
        public List<Interview> GetByEmployeeGuid(Guid employeeGuid)
        {
             return _context.Tests.Where(i => i.EmployeeGuid == employeeGuid).ToList();
        }
    }
}
