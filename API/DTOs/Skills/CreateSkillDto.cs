using API.Models;

namespace API.DTOs.Skills;

public class CreateSkillDto
{
    public string? Skill { get; set; }

    public static implicit operator Skill(CreateSkillDto skillDto)
    {
        return new Skill
        {
            Name = skillDto.Skill,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now

        };
    }
}
