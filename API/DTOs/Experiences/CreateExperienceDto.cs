using API.Models;

namespace API.DTOs.Experiences;

public class CreateExperienceDto
{
    public string Name { get; set; }
    public string Position { get; set; }
    public string Company { get; set; }



    public static implicit operator Experience(CreateExperienceDto experienceDto) 
    {
        return new Experience
        {
            Name = experienceDto.Name,
            Position = experienceDto.Position,
            Company = experienceDto.Company,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };
    }
}
