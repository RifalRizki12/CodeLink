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

        public double? GetAverageRatingByEmployee(Guid employeeGuid)
        {
            // Ambil semua rating dari interview yang terkait dengan employee tertentu
            var ratings = _context.Tests
                                  .Where(i => i.EmployeeGuid == employeeGuid)
                                  .Join(_context.Ratings,
                                        interview => interview.Guid,
                                        rating => rating.Guid,
                                        (interview, rating) => rating.Rate)
                                  .ToList();

            // Jika tidak ada rating yang ditemukan, kembalikan null
            if (!ratings.Any()) return null;

            // Hitung rata-rata rating
            return ratings.Average();
        }
    }
}
