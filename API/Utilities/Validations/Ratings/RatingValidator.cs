using API.DTOs.Ratings;
using FluentValidation;

namespace API.Utilities.Validations.Ratings
{
    public class RatingValidator : AbstractValidator<RatingDto>
    { 
    
        public RatingValidator()
        {
            RuleFor(e => e.Guid)
                .NotEmpty();

            RuleFor(e => e.Rate)
               .NotNull();
        }
    }
}
