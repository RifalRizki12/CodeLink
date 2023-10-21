using API.Contracts;
using API.Data;
using API.Models;

namespace API.Repositories
{
    public class RatingRepository : GeneralRepository<Rating>, IRatingRepository
    {
        public RatingRepository(CodeLinkDbContext context) : base(context) { }
    }
}
