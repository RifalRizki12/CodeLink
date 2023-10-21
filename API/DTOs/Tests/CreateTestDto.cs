using API.DTOs.Salaries;
using API.Models;
using static System.Net.Mime.MediaTypeNames;

namespace API.DTOs.Tests
{
    public class CreateTestDto
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public Guid EmployeeGuid { get; set; }


        public static implicit operator Test(CreateTestDto createTestDto)
        {
            return new Test
            {
                Name = createTestDto.Name,
                Date = createTestDto.Date,
                EmployeeGuid = createTestDto.EmployeeGuid,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };
        }
    }
}
