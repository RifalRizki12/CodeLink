﻿using API.DTOs.Experiences;
using API.DTOs.ExperienceSkills;
using FluentValidation;

namespace API.Utilities.Validations.ExperienceSkills;

public class CreateExperienceSkillValidator : AbstractValidator<CreateExperienceSkillDto>
{
    public CreateExperienceSkillValidator()
    {
       
        RuleFor(es => es.EmployeeGuid)
          .NotEmpty();


    }
}