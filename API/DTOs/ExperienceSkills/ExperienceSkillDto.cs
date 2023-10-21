using API.Models;

namespace API.DTOs.ExperienceSkills;

public class ExperienceSkillDto
{
    public Guid Guid { get; set; }
    public Guid? SkillGuid { get; set; }
    public Guid? ExperienceGuid { get; set; }
    public Guid EmployeeGuid { get; set; }



    public static explicit operator ExperienceSkillDto(ExperienceSkill expSkill)
    {
        return new ExperienceSkillDto
        {
           Guid = expSkill.Guid,
           SkillGuid = expSkill.SkillGuid,
           ExperienceGuid = expSkill.ExperienceGuid,
           EmployeeGuid = expSkill.EmployeeGuid,
        };
    }

    public static implicit operator ExperienceSkill(ExperienceSkillDto expSkillDto) 
    {
        return new ExperienceSkill
        {
            Guid = expSkillDto.Guid,
            SkillGuid = expSkillDto.SkillGuid,
            ExperienceGuid = expSkillDto.ExperienceGuid,
            EmployeeGuid = expSkillDto.EmployeeGuid,
            ModifiedDate = DateTime.Now

        };
    }
}
