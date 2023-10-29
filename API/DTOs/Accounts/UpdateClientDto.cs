using System;
using Microsoft.AspNetCore.Http;
using API.Utilities.Enums;
using API.Models;

namespace API.DTOs.Accounts
{
    public class UpdateClientDto
    {
        public Guid EmployeeGuid { get; set; }
        public Guid CompanyGuid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public GenderLevel Gender { get; set; }
        public IFormFile ProfilePictureFile { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string NameCompany { get; set; }
        public string AddressCompany { get; set; }
        public string Description { get; set; }



        // Konversi dari UpdateClientDto ke entitas yang relevan

        public static implicit operator Employee(UpdateClientDto dto)
        {
            return new Employee
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Gender = dto.Gender,

            };
        }

        public static implicit operator Company(UpdateClientDto dto)
        {
            return new Company
            {
                Name = dto.NameCompany,
                Address = dto.AddressCompany,
                Description = dto.Description,

            };
        }
    }
}
