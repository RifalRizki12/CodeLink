using API.Contracts;
using API.Data;
using API.Models;

namespace API.Repositories
{
    public class InterviewRepository : GeneralRepository<Interview>, IInterviewRepository
    {
        public InterviewRepository(CodeLinkDbContext context) : base(context) { }
    }
}
