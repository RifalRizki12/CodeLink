using API.Models;
using API.Utilities.Enums;

namespace API.DTOs.Interviews; 

public class GetIdleHistoryDto //untuk update interview yang biasa
{
    public Guid EmployeeGuid { get; set; }
    public Guid InterviewGuid { get; set; }
    public string Idle{ get; set; }
    public string Foto { get; set; }
    public string Rate { get; set; }
    public string? Remarks { get; set; }
    public StatusIntervew? StatusInterview { get; set; }
    public DateTime? StartContract { get; set; }
    public DateTime? EndContract { get; set; }


    public static explicit operator GetIdleHistoryDto(Employee employee)
    {
        return new GetIdleHistoryDto
        {
            EmployeeGuid = employee.Guid,
            Idle = employee.FirstName + " " + employee.LastName,
            Foto = employee.Foto,

        };
    }

    public static explicit operator GetIdleHistoryDto(Interview interview)
    {
        return new GetIdleHistoryDto
        {
            InterviewGuid = interview.Guid,
            StartContract = interview.StartContract,
            EndContract = interview.EndContract,
            StatusInterview=interview.StatusIntervew,
            Remarks= interview.Remarks
            

        };
    }
    public static explicit operator GetIdleHistoryDto(Rating rate)
    {
        return new GetIdleHistoryDto
        {
            Rate = rate.Rate.ToString(),

        };
    }
}


