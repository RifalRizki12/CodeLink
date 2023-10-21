using API.DTOs.Experiences;
using API.DTOs.ExperienceSkills;
using FluentValidation;

namespace API.Utilities.Validations.ExperienceSkills;

public class ExperienceSkillValidator : AbstractValidator<ExperienceSkillDto>
{
    public ExperienceSkillValidator()
    {
        RuleFor(es => es.Guid)
         .NotEmpty();

        RuleFor(es => es.EmployeeGuid)
          .NotEmpty();
    }
}
