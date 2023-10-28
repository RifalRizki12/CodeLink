using API.Contracts;
using API.Data;
using API.Models;

namespace API.Repositories
{
    public class SkillRepository : GeneralRepository<Skill>, ISkillRepository
    {
        public SkillRepository(CodeLinkDbContext context) : base(context) { }

        public List<Skill> GetSkillsByCvGuid(Guid cvGuid)
        {
            return _context.Skills.Where(s => s.CvGuid == cvGuid).ToList();
        }
        public void Add(Skill skill)
        {
            _context.Skills.Add(skill);
            _context.SaveChanges();
        }
    }
}
