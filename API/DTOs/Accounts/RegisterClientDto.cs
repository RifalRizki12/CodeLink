using API.Models;
using API.Utilities.Enums;

namespace API.DTOs.Accounts
{
    public class RegisterClientDto
    {
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public GenderLevel Gender { get; set; }
        public DateTime HiringDate { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string NameCompany { get; set; }
        public string AddressCompany { get; set; }
        public string Description { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public static implicit operator Account(RegisterClientDto accountDto)
        {
            return new Account
            {
                Guid = Guid.NewGuid(),
                Password = accountDto.ConfirmPassword,
                Otp = 0,
                IsUsed = true,
                Status = 0,
                ExpiredTime = DateTime.Now,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };
        }

        public static implicit operator Employee(RegisterClientDto registrationDto)
        {
            return new Employee
            {
                FirstName = registrationDto.FirstName,
                LastName = registrationDto.LastName,
                Gender = registrationDto.Gender,
                HiringDate = registrationDto.HiringDate,
                Email = registrationDto.Email,
                PhoneNumber = registrationDto.PhoneNumber,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };
        }
        public static implicit operator Company(RegisterClientDto registrationDto)
        {
            return new Company
            {
                Guid = Guid.NewGuid(),
                Name = registrationDto.NameCompany,
                Address = registrationDto.AddressCompany,
                Description = registrationDto.Description,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };
        }
    }
}
