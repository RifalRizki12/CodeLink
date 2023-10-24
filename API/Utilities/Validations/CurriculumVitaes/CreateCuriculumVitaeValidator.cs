using API.DTOs.Experiences;
using API.DTOs.ExperienceSkills;
using FluentValidation;

namespace API.Utilities.Validations.CurriculumVitaes;

public class CreateCurriculumVitaeValidator : AbstractValidator<CreateCurriculumVitaeDto>
{
    public CreateCurriculumVitaeValidator()
    {
       
/*        RuleFor(es => es.Gu)
          .NotEmpty();*/


    }
}
