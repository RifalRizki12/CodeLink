using API.Models;

namespace API.DTOs.Employees
{
    public class ClientDetailDto
    {
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



        public static explicit operator ClientDetailDto(Employee employee)
        {
            return new ClientDetailDto
            {
                /*Guid = employee.Guid,*/
                FullName = employee.FirstName + " " + employee.LastName,
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

                NameCompany = company.Name,
                Address = company.Address,
            };
        }



    }
}
