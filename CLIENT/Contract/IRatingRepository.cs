using API.DTOs.Ratings;
using API.Models;

namespace CLIENT.Contract
{
    public interface IRatingRepository : IRepository<RatingDto, Guid>
    {
 /*       Task<API.Utilities.Handler.ResponseOKHandler<RatingDto>> UpdateRating(Guid guid, RatingDto rating);*/
    }
}
