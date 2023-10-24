using API.Models;

namespace API.Contracts
{
    public interface IRatingRepository : IGeneralRepository<Rating>
    {
        /*double? GetAverageRatingByEmployeeAndCompany(Guid employeeGuid);*/
    }
}
