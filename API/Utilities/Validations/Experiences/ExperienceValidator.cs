using API.DTOs.Experiences;
using FluentValidation;

namespace API.Utilities.Validations.Experiences;

public class ExperienceValidator : AbstractValidator<ExperienceDto>
{
    public ExperienceValidator()
    {

        RuleFor(e => e.Name)
            .MaximumLength(100);

        RuleFor(e => e.Position)
            .MaximumLength(50);

        RuleFor(e => e.Company)
            .MaximumLength(50);
    }
}
