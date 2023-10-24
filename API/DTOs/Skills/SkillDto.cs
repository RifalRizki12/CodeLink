using API.Models;

namespace API.DTOs.Skills;

public class SkillDto
{
    public Guid Guid { get; set; }
    public string? Skill { get; set; }

    public static explicit operator SkillDto(Skill skill)
    {
        return new SkillDto
        {
           Guid = skill.Guid,
           Skill = skill.Name,
        };
    }

    public static implicit operator Skill(SkillDto skillDto) 
    {
        return new Skill
        {
            Guid = skillDto.Guid,
            Name = skillDto.Skill,
            ModifiedDate = DateTime.Now

        };
    }
}
