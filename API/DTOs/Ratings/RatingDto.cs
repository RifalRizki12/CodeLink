using API.DTOs.Salaries;
using API.Models;

namespace API.DTOs.Ratings
{
    public class RatingDto
    {
        public Guid Guid { get; set; }
        public int Rate { get; set; }
        public Guid EmployeeGuid { get; set; }



        public static explicit operator RatingDto(Rating rating)
        {
            return new RatingDto
            {
                Guid = rating.Guid,
                Rate = rating.Rate,
                EmployeeGuid = rating.EmployeeGuid,
            };
        }

        public static implicit operator Rating(RatingDto ratingDto)
        {
            return new Rating
            {
                Guid = ratingDto.Guid,
                Rate = ratingDto.Rate,
                EmployeeGuid = ratingDto.EmployeeGuid,
                ModifiedDate = DateTime.Now
            };
        }
    }
}
