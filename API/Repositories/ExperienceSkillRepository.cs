using API.Contracts;
using API.Data;
using API.Models;

namespace API.Repositories
{
    public class ExperienceSkillRepository : GeneralRepository<ExperienceSkill>, IExperienceSkillRepository
    {
        public ExperienceSkillRepository(CodeLinkDbContext context) : base(context) { }
    }
}
