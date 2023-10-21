using API.DTOs.Salaries;
using API.Models;

namespace API.DTOs.Ratings
{
    public class CreateRatingDto
    {
        public int Rate { get; set; }
        public Guid EmployeeGuid { get; set; }


        public static implicit operator Rating(CreateRatingDto createRatingDto)
        {
            return new Rating
            {
                Rate = createRatingDto.Rate,
                EmployeeGuid = createRatingDto.EmployeeGuid,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };
        }
    }
}

