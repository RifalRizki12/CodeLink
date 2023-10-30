using API.DTOs.Companies;
using API.Models;
using API.Utilities.Enums;

namespace API.DTOs.Employees
{
    public class EmployeeDetailDto
    {
        public Guid Guid { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string StatusEmployee { get; set; }
        public string NameCompany { get; set; }
        public string? Foto { get; set; }
        public string Grade { get; set; }
        public string? Cv { get; set; }
        public string Address { get; set; }
        public List<string>? Skill { get; set; }
        public string EmployeeOwner { get; set; }
        public Guid OwnerGuid { get; set; }
        public double? AverageRating { get; set; }


        // Operator eksplisit untuk mengkonversi dari model entitas ke DTO
        public static explicit operator EmployeeDetailDto(Employee employee)
        {
            return new EmployeeDetailDto
            {
                Guid = employee.Guid,
                FullName = employee.FirstName + " " + employee.LastName,
                FirstName = employee.FirstName,
                LastName= employee.LastName,
                Gender = employee.Gender.ToString(),
                Email = employee.Email,
                PhoneNumber = employee.PhoneNumber,
                StatusEmployee = employee.StatusEmployee.ToString(),
                Foto = employee.Foto,
                Grade = employee.Grade.ToString(),
                EmployeeOwner = employee.FirstName + " " + employee.LastName,
                OwnerGuid = employee.Guid,
            };
        }

        public static explicit operator EmployeeDetailDto(Company company)
        {
            return new EmployeeDetailDto
            {
                NameCompany = company.Name,
                Address = company.Address,

            };
        }

        public static explicit operator EmployeeDetailDto(Rating rating)
        {
            return new EmployeeDetailDto
            {
                AverageRating = rating.Rate
            };
        }
        
        public static explicit operator EmployeeDetailDto(CurriculumVitae cv)
        {
            return new EmployeeDetailDto
            {
                Cv = cv.Cv,
            };
        }
        
        public static explicit operator EmployeeDetailDto(Skill skill)
        {
            return new EmployeeDetailDto
            {
                Skill = new List<string> { skill.Name }
            };
        }
    }
}
