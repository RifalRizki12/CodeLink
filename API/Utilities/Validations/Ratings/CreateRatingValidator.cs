using API.DTOs.Ratings;
using FluentValidation;

namespace API.Utilities.Validations.Ratings
{
    public class CreateRatingValidator : AbstractValidator<CreateRatingDto>
    {
        public CreateRatingValidator()
        {
            RuleFor(e => e.Rate)
                .NotNull();

            RuleFor(e => e.EmployeeGuid)
                .NotEmpty();
        }
    }
}
