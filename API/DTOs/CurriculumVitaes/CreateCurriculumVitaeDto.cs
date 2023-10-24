using API.Models;

namespace API.DTOs.ExperienceSkills;

public class CreateCurriculumVitaeDto
{
    public Guid SkillGuid { get; set; }
    public Guid ExperienceGuid { get; set; }

    public static implicit operator CurriculumVitae(CreateCurriculumVitaeDto Dto)
    {
        return new CurriculumVitae
        {
            SkillGuid = Dto.SkillGuid,
            ExperienceGuid = Dto.ExperienceGuid,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now

        };
    }

}
