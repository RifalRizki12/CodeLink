using API.Models;
using CLIENT.Contract;

namespace CLIENT.Repository
{
    public class ExperienceRepository : GeneralRepository<Experience, Guid>, IExperienceRepository
    {

        public ExperienceRepository(string request = "Experience/") : base(request)
        {

        }

    }
}
