﻿using API.Models;
using API.Utilities.Enums;

namespace API.DTOs.Employees
{
    public class EmployeeDto
    {
        public Guid Guid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public GenderLevel Gender { get; set; }
        public DateTime? HireDate { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Status { get; set; }

        // Konversi Eksplisit (Explicit Conversion):
        // Metode ini akan mengonversi EmployeeDto ke Employee secara eksplisit jika diperlukan.
        public static explicit operator EmployeeDto(Employee employee)
        {

            return new EmployeeDto
            {
                Guid = employee.Guid,               // Mengonversi GUID dari Employee ke EmployeeDto.
                FirstName = employee.FirstName,     // Mengonversi Nama Depan dari Employee ke EmployeeDto.
                LastName = employee.LastName,       // Mengonversi Nama Belakang dari Employee ke EmployeeDto.
                Gender = employee.Gender,           // Mengonversi Jenis Kelamin dari Employee ke EmployeeDto.
                HireDate = employee.HireDate,
                ExpiredDate = employee.ExpiredDate,
                Status = employee.Status,
                Email = employee.Email,             // Mengonversi Email dari Employee ke EmployeeDto.
                PhoneNumber = employee.PhoneNumber  // Mengonversi Nomor Telepon dari Employee ke EmployeeDto.
            };
        }

        // Konversi Implisit (Implicit Conversion):
        // Metode ini akan mengonversi EmployeeDto ke Employee secara implisit jika diperlukan.
        public static implicit operator Employee(EmployeeDto dto)
        {
            return new Employee
            {
                Guid = dto.Guid,                // Mengonversi GUID dari EmployeeDto ke Employee.
                FirstName = dto.FirstName,      // Mengonversi Nama Depan dari EmployeeDto ke Employee.
                LastName = dto.LastName,        // Mengonversi Nama Belakang dari EmployeeDto ke Employee.
                Gender = dto.Gender,            // Mengonversi Jenis Kelamin dari EmployeeDto ke Employee.
                HireDate = dto.HireDate,
                ExpiredDate = dto.ExpiredDate,
                Status = dto.Status,
                Email = dto.Email,              // Mengonversi Email dari EmployeeDto ke Employee.
                PhoneNumber = dto.PhoneNumber,   // Mengonversi Nomor Telepon dari EmployeeDto ke Employee.
                ModifiedDate = DateTime.Now
            };
        }
    }
}
