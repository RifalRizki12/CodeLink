using API.DTOs.Experiences;
using FluentValidation;

namespace API.Utilities.Validations.Experiences;

public class CreateExperienceSkillValidator : AbstractValidator<CreateExperienceDto>
{
    public CreateExperienceSkillValidator()
    {
        RuleFor(e => e.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(e => e.Position)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(e => e.Company)
            .NotEmpty()
            .MaximumLength(50);
    }
}
