using API.DTOs.Accounts;
using API.Models;
using API.Utilities.Enums;

namespace API.DTOs.Accounts
{
    public class UpdateIdleDto
    {
        public Guid Guid { get; set; }
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public GenderLevel Gender { get; set; }
        public IFormFile? ProfilePictureFile { get; set; }
        public IFormFile? CvFile { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public GradeLevel Grade { get; set; }
        public StatusEmployee StatusEmployee { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public Guid? CompanyGuid { get; set; }
        public List<string>? Skills { get; set; }

        public static implicit operator Employee(UpdateIdleDto dto)
        {
            return new Employee
            {

                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Gender = dto.Gender,
                Foto = null,
                PhoneNumber = dto.PhoneNumber,
                Grade = dto.Grade,
                Email = dto.Email,
                StatusEmployee = dto.StatusEmployee,
                CompanyGuid= dto.CompanyGuid
            };
        }

        public static implicit operator Account(UpdateIdleDto dto)
        {
            return new Account
            {

                Password = dto.ConfirmPassword,
                Otp = 0,
                IsUsed = true,
                Status = StatusLevel.Approved,
                ExpiredTime = DateTime.Now,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };
        }

        public static implicit operator CurriculumVitae(UpdateIdleDto dto)
        {
            return new CurriculumVitae
            {
                Cv = null,
            };
        }


        public static implicit operator List<Skill>(UpdateIdleDto dto)
        {
            
            var skillsList = new List<Skill>();

            foreach (var skillName in dto.Skills)
            {
                var skill = new Skill
                {

                    Name = skillName
                };
                skillsList.Add(skill);
            }

            return skillsList;
        }
    }
}

