﻿using API.Models;
using API.Utilities.Enums;

namespace API.DTOs.Employees
{
    public class CreateEmployeeDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public GenderLevel Gender { get; set; }
        public DateTime? HireDate { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public string Status { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        // Konversi Implisit (Implicit Conversion):
        // Metode ini akan mengonversi EmployeeDto ke Employee secara implisit jika diperlukan.
        public static implicit operator Employee(CreateEmployeeDto dto)
        {
            // Dalam metode ini, menginisialisasi objek Employee
            // menggunakan nilai-nilai dari objek CreateEmployeeDto yang sesuai.
            return new Employee
            {
                // Properti seperti Nik dari objek Employee diisi dengan nilai dari beberapa properti
                // dari objek CreateEmployeeDto (dto.Nik).
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Gender = dto.Gender,
                HireDate = dto.HireDate,
                ExpiredDate = dto.ExpiredDate,
                Status = dto.Status,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };
        }
    }
}
