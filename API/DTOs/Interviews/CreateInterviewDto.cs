
using API.Models;
using static System.Net.Mime.MediaTypeNames;

namespace API.DTOs.Interviews
{
    public class CreateInterviewDto
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public DateTime? StartContract {  get; set; }
        public DateTime? EndContract {  get; set; }
        public Guid EmployeeGuid { get; set; }


        public static implicit operator Interview(CreateInterviewDto createInterviewDto)
        {
            return new Interview
            {
                Name = createInterviewDto.Name,
                Date = createInterviewDto.Date,
                EmployeeGuid = createInterviewDto.EmployeeGuid,
                StartContract = createInterviewDto.StartContract,
                EndContract = createInterviewDto.EndContract,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };
        }
    }
}
