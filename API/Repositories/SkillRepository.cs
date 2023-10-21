using API.Contracts;
using API.Data;
using API.Models;

namespace API.Repositories
{
    public class SkillRepository : GeneralRepository<Skill>, ISkillRepository
    {
        public SkillRepository(CodeLinkDbContext context) : base(context) { }
    }
}
