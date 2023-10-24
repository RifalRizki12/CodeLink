using API.DTOs.Experiences;
using API.DTOs.ExperienceSkills;
using FluentValidation;

namespace API.Utilities.Validations.CurriculumVitaes;

public class CurriculumVitaeValidator : AbstractValidator<CurriculumVitaeDto>
{
    public CurriculumVitaeValidator()
    {
        RuleFor(es => es.Guid)
         .NotEmpty();
    }
}
