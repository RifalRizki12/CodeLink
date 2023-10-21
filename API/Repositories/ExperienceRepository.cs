using API.Contracts;
using API.Data;
using API.Models;

namespace API.Repositories
{
    public class ExperienceRepository : GeneralRepository<Experience>, IExperienceRepository
    {
        public ExperienceRepository(CodeLinkDbContext context) : base(context) { }
    }
}
