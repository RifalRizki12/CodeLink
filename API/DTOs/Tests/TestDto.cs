using API.DTOs.Salaries;
using API.Models;
using static System.Net.Mime.MediaTypeNames;

namespace API.DTOs.Tests
{
    public class TestDto
    {
        public Guid Guid { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public Guid EmployeeGuid { get; set; }



        public static explicit operator TestDto(Test test)
        {
            return new TestDto
            {
                Guid = test.Guid,
                Name = test.Name,
                Date = test.Date,
                EmployeeGuid = test.EmployeeGuid
            };
        }

        public static implicit operator Test(TestDto testDto)
        {
            return new Test
            {
                Guid = testDto.Guid,
                Name = testDto.Name,
                Date = testDto.Date,
                EmployeeGuid = testDto.EmployeeGuid,
                ModifiedDate = DateTime.Now
            };
        }
    }
}
