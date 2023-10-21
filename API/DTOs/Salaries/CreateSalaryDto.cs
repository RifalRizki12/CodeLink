using API.DTOs.Salaries;
using API.Models;

namespace API.DTOs.Salaries
{
    public class CreateSalaryDto
    {
        public int BasicSalary { get; set; }
        public int? OvertimePay { get; set; }


        public static implicit operator Salary(CreateSalaryDto createSalaryDto)
        {
            return new Salary
            {
                BasicSalary = createSalaryDto.BasicSalary,
                OvertimePay = createSalaryDto.OvertimePay,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };
        }
    }
}
