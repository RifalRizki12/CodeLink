using API.DTOs.Companies;
using API.Models;
using API.Utilities.Enums;

namespace API.DTOs.Employees
{
    public class EmployeeDetailDto
    {
        public string FullName { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string StatusEmployee { get; set; }
        public string NameCompany { get; set; }
        public string? Foto { get; set; }
        public string Grade { get; set; }
        public DateTime? HireMetro { get; set; }
        public DateTime? EndMetro { get; set; }
        public string? Cv { get; set; }
        public string Address { get; set; }
        public List<string> Skill { get; set; }
        public List<string> Experience { get; set; }
        public string EmployeeOwner { get; set; }
        public Guid OwnerGuid { get; set; }
        public double? AverageRating { get; set; }


        public static explicit operator EmployeeDetailDto(Employee employee)
        {
            return new EmployeeDetailDto
            {
               
                FullName = employee.LastName + employee.FirstName,       // Mengonversi Nama Belakang dari Employee ke EmployeeDto.
                Gender = employee.Gender.ToString(),
                Grade = employee.Grade.ToString(),
                StatusEmployee = employee.StatusEmployee.ToString(),
                Email = employee.Email,             // Mengonversi Email dari Employee ke EmployeeDto.
                PhoneNumber = employee.PhoneNumber,  
                OwnerGuid =employee.Guid,
                EmployeeOwner = employee.FirstName
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


    }
}
