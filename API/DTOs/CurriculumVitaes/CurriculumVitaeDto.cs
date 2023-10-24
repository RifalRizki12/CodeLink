using API.Models;

namespace API.DTOs.ExperienceSkills;

public class CurriculumVitaeDto
{
    public Guid Guid { get; set; }

    public static explicit operator CurriculumVitaeDto(CurriculumVitae dto)
    {
        return new CurriculumVitaeDto
        {
           Guid = dto.Guid,
        };
    }

    public static implicit operator CurriculumVitae(CurriculumVitaeDto expSkillDto) 
    {
        return new CurriculumVitae
        {
            Guid = expSkillDto.Guid,
            ModifiedDate = DateTime.Now

        };
    }
}
