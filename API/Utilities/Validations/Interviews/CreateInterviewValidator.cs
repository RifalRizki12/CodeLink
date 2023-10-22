using API.DTOs.Interviews;
using FluentValidation;

namespace API.Utilities.Validations.Interviews
{
    public class CreateInterviewValidator : AbstractValidator<CreateInterviewDto>
    {
        public CreateInterviewValidator()
        {
            RuleFor(e => e.Name)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(e => e.Date)
                .NotEmpty();

            RuleFor(e => e.EmployeeGuid)
                .NotEmpty();
        }
    }
}
