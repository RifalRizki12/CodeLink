using API.DTOs.Experiences;
using API.DTOs.Skills;
using FluentValidation;

namespace API.Utilities.Validations.Skills;

public class CreateSkillValidator : AbstractValidator<CreateSkillDto>
{
    public CreateSkillValidator()
    {
        RuleFor(s => s.Hard)
            .MaximumLength(50);

        RuleFor(s => s.Soft)
            .MaximumLength(50);
    }
}
