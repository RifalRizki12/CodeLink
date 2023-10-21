using API.DTOs.Roles;
using API.Models;

namespace API.DTOs.Salaries
{
    public class SalaryDto
    {
        public Guid Guid { get; set; }
        public int BasicSalary { get; set; }
        public int OvertimePay { get; set; }



        public static explicit operator SalaryDto(Salary salary)
        {
            return new SalaryDto
            {
                Guid = salary.Guid,
                BasicSalary = salary.BasicSalary,
                OvertimePay = salary.OvertimePay,
            };
        }

        public static implicit operator Salary(SalaryDto salaryDto)
        {
            return new Salary
            {
                Guid = salaryDto.Guid,       
                BasicSalary = salaryDto.BasicSalary,
                OvertimePay = salaryDto.OvertimePay,
                ModifiedDate = DateTime.Now
            };
        }
    }
}
