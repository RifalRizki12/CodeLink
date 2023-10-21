using API.DTOs.Interviews;
using FluentValidation;

namespace API.Utilities.Validations.Test
{
    public class TestValidator : AbstractValidator<InterviewDto>
    {
        public TestValidator()
        {
            RuleFor(e => e.Guid)
                .NotEmpty();

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
