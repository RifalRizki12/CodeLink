using API.Models;

namespace API.DTOs.Ratings
{
    public class CreateRatingDto
    {
        public int? Rate { get; set; }
        public string? Feedback { get; set; }


        public static implicit operator Rating(CreateRatingDto createRatingDto)
        {
            return new Rating
            {
                Rate = createRatingDto.Rate,
                Feedback = createRatingDto.Feedback,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };
        }
    }
}

