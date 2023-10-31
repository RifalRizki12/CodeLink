using API.Models;
using API.Utilities.Enums;

namespace API.DTOs.Interviews
{
    public class ScheduleInterviewDto //untuk admin kirim email detail schedule interview
    {
        public Guid Guid { get; set; }
        public string? Location { get; set; }
        public TypeInterview? Type { get; set; }
        public string? Remarks { get; set; }
        public Guid EmployeeGuid { get; set; }
        public Guid OwnerGuid { get; set; }

     



        public static explicit operator ScheduleInterviewDto(Interview interview)
        {
            return new ScheduleInterviewDto
            {
                Guid = interview.Guid,
                Location = interview.Location,
                Type = interview.Type,
                Remarks = interview.Remarks,
                EmployeeGuid = interview.EmployeeGuid,
                OwnerGuid = interview.OwnerGuid,
            };
        }

        public static implicit operator Interview(ScheduleInterviewDto interviewDto)
        {
            return new Interview
            {
                Guid = interviewDto.Guid,
                Location = interviewDto.Location,
                Type = interviewDto.Type,
                Remarks = interviewDto.Remarks,
                EmployeeGuid = interviewDto.EmployeeGuid,
                OwnerGuid = interviewDto.OwnerGuid,
            };
        }
    }
}
