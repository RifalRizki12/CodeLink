using API.Models;

namespace API.DTOs.Experiences;

public class ExperienceDto
{
    public Guid Guid { get; set; }
    public string Name { get; set; }
    public string Position { get; set; }
    public string Company { get; set; }



    public static explicit operator ExperienceDto(Experience experience)
    {
        return new ExperienceDto
        {
            Guid = experience.Guid,
            Name = experience.Name,
            Position = experience.Position,
            Company = experience.Company,
        };
    }

    public static implicit operator Experience(ExperienceDto experienceDto) 
    {
        return new Experience
        {
            Guid = experienceDto.Guid,
            Name = experienceDto.Name,
            Position = experienceDto.Position,
            Company = experienceDto.Company,
            ModifiedDate = DateTime.Now
        };
    }
}
