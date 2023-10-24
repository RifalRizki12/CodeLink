using API.Models;

namespace API.DTOs.Ratings
{
    public class RatingDto
    {
        public Guid Guid { get; set; }
        public int? Rate { get; set; }
        public string? Feedback { get; set; }



        public static explicit operator RatingDto(Rating rating)
        {
            return new RatingDto
            {
                Guid = rating.Guid,
                Rate = rating.Rate,
                Feedback = rating.Feedback,
            };
        }

        public static implicit operator Rating(RatingDto ratingDto)
        {
            return new Rating
            {
                Guid = ratingDto.Guid,
                Rate = ratingDto.Rate,
                Feedback = ratingDto.Feedback,
                ModifiedDate = DateTime.Now
            };
        }
    }
}
