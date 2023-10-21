using API.Models;

namespace API.DTOs.Skills;

public class SkillDto
{
    public Guid Guid { get; set; }
    public string? Hard { get; set; }
    public string? Soft { get; set; }
    



    public static explicit operator SkillDto(Skill skill)
    {
        return new SkillDto
        {
           Guid = skill.Guid,
           Hard = skill.Hard,
           Soft = skill.Soft,
        };
    }

    public static implicit operator Skill(SkillDto skillDto) 
    {
        return new Skill
        {
            Guid = skillDto.Guid,
            Hard = skillDto.Hard,
            Soft = skillDto.Soft,
            ModifiedDate = DateTime.Now

        };
    }
}
