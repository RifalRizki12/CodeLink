using API.Models;
using API.Utilities.Enums;

namespace API.DTOs.Interviews; 

public class GetOnsiteDto //untuk update interview yang biasa
{
    public Guid EmployeGuid { get; set; }
    public DateTime? StartContract { get; set; }
    public DateTime? EndContract { get; set; }
    public string Idle { get; set; }
    public string? Foto { get; set; }
    public static explicit operator GetOnsiteDto(Employee employee)
    {
        return new GetOnsiteDto
        {
            EmployeGuid = employee.Guid,
            Idle = employee.FirstName + " " + employee.LastName,
            Foto = employee.Foto,
        };
    }

    public static explicit operator GetOnsiteDto(Interview interview)
    {
        return new GetOnsiteDto
        {

            StartContract = interview.StartContract,
            EndContract = interview.EndContract,

        };
    }
}


