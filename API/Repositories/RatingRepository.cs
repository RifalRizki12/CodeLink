using API.Contracts;
using API.Data;
using API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace API.Repositories
{
    public class RatingRepository : GeneralRepository<Rating>, IRatingRepository
    {
        private readonly CodeLinkDbContext _context;

        public RatingRepository(CodeLinkDbContext context) : base(context)
        {
            _context = context;
        }

/*        public double? GetAverageRatingByEmployeeAndCompany(Guid employeeGuid)
        {
            // Query menggunakan Entity Framework
            return _context.Ratings
                .Where(r => r.EmployeeGuid == employeeGuid)
                .Average(r => (double?)r.Rate);

        }*/
    }
}
