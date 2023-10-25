using API.Models;

namespace API.DTOs.ExperienceSkills;

public class CurriculumVitaeDto
{
    public Guid Guid { get; set; }
    public string? Cv { get; set; }

    public static explicit operator CurriculumVitaeDto(CurriculumVitae dto)
    {
        return new CurriculumVitaeDto
        {
           Guid = dto.Guid,
           Cv = dto.Cv,
        };
    }

    public static implicit operator CurriculumVitae(CurriculumVitaeDto expSkillDto) 
    {
        return new CurriculumVitae
        {
            Guid = expSkillDto.Guid,
            Cv = expSkillDto.Cv,
            ModifiedDate = DateTime.Now

        };
    }
}
