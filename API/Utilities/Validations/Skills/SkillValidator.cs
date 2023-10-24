﻿using API.DTOs.Experiences;
using API.DTOs.Skills;
using FluentValidation;

namespace API.Utilities.Validations.Skills;

public class SkillValidator : AbstractValidator<SkillDto>
{
    public SkillValidator()
    {
        RuleFor(s => s.Guid)
            .NotEmpty();

        RuleFor(s => s.Skill)
            .MaximumLength(50);

    }
}
