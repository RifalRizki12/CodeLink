﻿using API.Models;

namespace API.Contracts
{
    public interface IRatingRepository : IGeneralRepository<Rating>
    {
        double? GetAverageRatingByEmployee(Guid employeeGuid);

        List<Rating> GetByInterviewGuid(Guid interviewGuid);

    }
}
