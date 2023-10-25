using API.Models;

namespace API.DTOs.ExperienceSkills;

public class CreateCurriculumVitaeDto
{
    public string? Cv { get; set; }

    public static implicit operator CurriculumVitae(CreateCurriculumVitaeDto Dto)
    {
        return new CurriculumVitae
        {
            Cv = Dto.Cv,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now

        };
    }

}
