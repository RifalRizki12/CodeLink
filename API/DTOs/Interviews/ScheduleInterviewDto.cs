using API.Models;
using static System.Net.Mime.MediaTypeNames;

namespace API.DTOs.Interviews
{
    public class ScheduleInterviewDto //untuk admin kirim email detail schedule interview
    {
        public Guid Guid { get; set; }
      
        public string Description { get; set; }
        public Guid EmployeeGuid { get; set; }
        public Guid OwnerGuid { get; set; }

     



        public static explicit operator ScheduleInterviewDto(Interview interview)
        {
            return new ScheduleInterviewDto
            {
                Guid = interview.Guid,
                Description = interview.Description,
                EmployeeGuid = interview.EmployeeGuid,
                OwnerGuid = interview.OwnerGuid,
            };
        }

        public static implicit operator Interview(ScheduleInterviewDto interviewDto)
        {
            return new Interview
            {
                Guid = interviewDto.Guid,
                Description = interviewDto.Description,
                EmployeeGuid = interviewDto.EmployeeGuid,
                OwnerGuid = interviewDto.OwnerGuid,
                ModifiedDate = DateTime.Now
            };
        }
    }
}
