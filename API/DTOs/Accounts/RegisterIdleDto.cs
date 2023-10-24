using API.Models;
using API.Utilities.Enums;
using API.Utilities.Handler;
using System.Reflection;

namespace API.DTOs.Accounts
{
    public class RegisterIdleDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public GenderLevel Gender { get; set; }
        public IFormFile? ProfilePictureFile { get; set; }
        public IFormFile? CvFile { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? HireMetro { get; set; }
        public DateTime? EndMetro { get; set; }
        public string Email { get; set; }
        public GradeLevel Grade { get; set; }
        public StatusEmployee StatusEmployee { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string ExperienceName { get; set; }
        public string Position { get; set; }
        public string Company { get; set; }
        public List<string> Skills { get; set; }

        public static implicit operator Employee(RegisterIdleDto dto)
        {
            return new Employee
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Gender = dto.Gender,
                Foto = null,
                PhoneNumber = dto.PhoneNumber,
                Grade = dto.Grade,
                HireMetro = dto.HireMetro,
                EndMetro = dto.EndMetro,
                Email = dto.Email,
                StatusEmployee = dto.StatusEmployee
            };
        }

        public static implicit operator Account(RegisterIdleDto dto)
        {
            return new Account
            {
                Guid = Guid.NewGuid(),
                Password = dto.ConfirmPassword,
                Otp = 0,
                IsUsed = true,
                Status = StatusLevel.Approved,
                ExpiredTime = DateTime.Now,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };
        }

        public static implicit operator Experience(RegisterIdleDto dto)
        {
            return new Experience
            {
                Guid = Guid.NewGuid(),
                Name = dto.ExperienceName,
                Position = dto.Position,
                Company = dto.Company
            };
        }
        
        public static implicit operator CurriculumVitae(RegisterIdleDto dto)
        {
            return new CurriculumVitae
            {
                Cv = null,
            };
        }

        public static implicit operator List<Skill>(RegisterIdleDto dto)
        {
            var skillsList = new List<Skill>();

            foreach (var skillName in dto.Skills)
            {
                var skill = new Skill
                {
                    Guid = Guid.NewGuid(),
                    Name = skillName
                };
                skillsList.Add(skill);
            }

            return skillsList;
        }
    }
}
