using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Contracts
{
    public interface ISkillRepository : IGeneralRepository<Skill>
    {
        List<Skill> GetSkillsByCvGuid(Guid cvGuid);
        void Add(Skill skill);
    }
}
