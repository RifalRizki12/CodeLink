using API.Models;
using static System.Net.Mime.MediaTypeNames;

namespace API.DTOs.Interviews
{
    public class InterviewDto
    {
        public Guid Guid { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public DateTime? StartContract { get; set; }
        public DateTime? EndContract { get; set; }
        public Guid EmployeeGuid { get; set; }
     



        public static explicit operator InterviewDto(Interview interview)
        {
            return new InterviewDto
            {
                Guid = interview.Guid,
                Name = interview.Name,
                Date = interview.Date,
                Description = interview.Description,
                StartContract = interview.StartContract,
                EndContract = interview.EndContract,
                EmployeeGuid = interview.EmployeeGuid
            };
        }

        public static implicit operator Interview(InterviewDto interviewDto)
        {
            return new Interview
            {
                Guid = interviewDto.Guid,
                Name = interviewDto.Name,
                Date = interviewDto.Date,
                Description= interviewDto.Description,
                StartContract = interviewDto.StartContract,
                EndContract = interviewDto.EndContract,
                EmployeeGuid = interviewDto.EmployeeGuid,
                ModifiedDate = DateTime.Now
            };
        }
    }
}
