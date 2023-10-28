using API.Models;

namespace API.DTOs.Employees;

public class ClientDetailDto
{
    //inisiasi properti yang akan digunakan 
    public Guid EmployeeGuid { get; set; }
    public string FirstName { get; set; }
    public string? LastName { get; set; }
    public string FullName { get; set; }
    public string Gender { get; set; }
    public string FotoEmployee { get; set; }
    public string Email { get; set; }
    public string StatusAccount { get; set; }
    public string PhoneNumber { get; set; }
    public string StatusEmployee { get; set; }
    public string NameCompany { get; set; }
    public string Address { get; set; }
    public string RoleName { get; set; }
    public Guid CompanyGuid { get; set; }
    public string? Description { get; set; }



    public static explicit operator ClientDetailDto(Employee employee)
    {
        return new ClientDetailDto
        {
            EmployeeGuid = employee.Guid,
            FullName = employee.FirstName + " " + employee.LastName,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Gender = employee.Gender.ToString(),
            StatusEmployee = employee.StatusEmployee.ToString(),
            Email = employee.Email,
            PhoneNumber = employee.PhoneNumber,
        };
    }

    public static explicit operator ClientDetailDto(Account account)
    {
        return new ClientDetailDto
        {
            StatusAccount = account.Status.ToString(),
        };
    }

    public static explicit operator ClientDetailDto(Company company)
    {
        return new ClientDetailDto
        {

            CompanyGuid = company.Guid,
            NameCompany = company.Name,
            Address = company.Address,
            Description = company.Description
        };
    }

    public static explicit operator ClientDetailDto(Role role)
    {
        return new ClientDetailDto
        {

            RoleName = role.Name,
        };
    }

}
