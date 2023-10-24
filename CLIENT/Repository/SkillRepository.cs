using API.Models;
using CLIENT.Contract;

namespace CLIENT.Repository
{
    public class SkillRepository : GeneralRepository<Skill, Guid>, ISkillRepository
    {

        public SkillRepository(string request = "Skill/") : base(request)
        {

        }

    }
}
