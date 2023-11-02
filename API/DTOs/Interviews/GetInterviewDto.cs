using API.Models;
using API.Utilities.Enums;

namespace API.DTOs.Interviews; 

public class GetInterviewDto //untuk update interview yang biasa
{
    public Guid EmployeGuid { get; set; }
    public Guid InterviewGuid { get; set; }
    public DateTime Date { get; set; }
    public string Idle { get; set; }
    public string? Foto { get; set; }
    public static explicit operator GetInterviewDto(Employee employee)
    {
        return new GetInterviewDto
        {
            EmployeGuid = employee.Guid,
            Idle = employee.FirstName + " " + employee.LastName,
            Foto = employee.Foto,
        };
    }

    public static explicit operator GetInterviewDto(Interview interview)
    {
        return new GetInterviewDto
        {

            Date = interview.Date,
            InterviewGuid = interview.Guid,

        };
    }
}


