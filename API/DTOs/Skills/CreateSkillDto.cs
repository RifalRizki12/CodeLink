using API.Models;

namespace API.DTOs.Skills;

public class CreateSkillDto
{
    public string? Hard { get; set; }
    public string? Soft { get; set; }




    public static implicit operator Skill(CreateSkillDto skillDto)
    {
        return new Skill
        {
            Hard = skillDto.Hard,
            Soft = skillDto.Soft,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now

        };
    }
}
