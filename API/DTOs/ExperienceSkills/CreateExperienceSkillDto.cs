﻿using API.Models;

namespace API.DTOs.ExperienceSkills;

public class CreateExperienceSkillDto
{
    public Guid Guid { get; set; }
    public Guid SkillGuid { get; set; }
    public Guid ExperienceGuid { get; set; }
    public Guid EmployeeGuid { get; set; }




    public static implicit operator ExperienceSkill(CreateExperienceSkillDto expSkillDto)
    {
        return new ExperienceSkill
        {
            Guid = expSkillDto.Guid,
            SkillGuid = expSkillDto.SkillGuid,
            ExperienceGuid = expSkillDto.ExperienceGuid,
            EmployeeGuid = expSkillDto.EmployeeGuid,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now

        };
    }

}