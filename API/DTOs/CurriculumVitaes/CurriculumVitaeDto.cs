using API.Models;

namespace API.DTOs.ExperienceSkills;

public class CurriculumVitaeDto
{
    public Guid Guid { get; set; }
    public Guid? SkillGuid { get; set; }
    public Guid? ExperienceGuid { get; set; }
    public Guid EmployeeGuid { get; set; }



    public static explicit operator CurriculumVitaeDto(CurriculumVitae expSkill)
    {
        return new CurriculumVitaeDto
        {
           Guid = expSkill.Guid,
           SkillGuid = expSkill.SkillGuid,
           ExperienceGuid = expSkill.ExperienceGuid,
        };
    }

    public static implicit operator CurriculumVitae(CurriculumVitaeDto expSkillDto) 
    {
        return new CurriculumVitae
        {
            Guid = expSkillDto.Guid,
            SkillGuid = expSkillDto.SkillGuid,
            ExperienceGuid = expSkillDto.ExperienceGuid,
            ModifiedDate = DateTime.Now

        };
    }
}
