﻿using API.Models;
using API.Utilities.Enums;
using System.Reflection.Metadata;

namespace API.DTOs.Employees
{
    public class EmployeeDto
    {
        public Guid Guid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public GenderLevel Gender { get; set; }
        public GradeLevel? GradeLevel { get; set; }
        public string? Foto { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public StatusEmployee StatusEmployee { get; set; }
        public Guid? CompanyGuid { get; set; }

        // Konversi Eksplisit (Explicit Conversion):
        // Metode ini akan mengonversi EmployeeDto ke Employee secara eksplisit jika diperlukan.
        public static explicit operator EmployeeDto(Employee employee)
        {
            return new EmployeeDto
            {
                Guid = employee.Guid,               // Mengonversi GUID dari Employee ke EmployeeDto.
                FirstName = employee.FirstName,     // Mengonversi Nama Depan dari Employee ke EmployeeDto.
                LastName = employee.LastName,       // Mengonversi Nama Belakang dari Employee ke EmployeeDto.
                Gender = employee.Gender,
                Foto = employee.Foto,               // Mengonversi Jenis Kelamin dari Employee ke EmployeeDto.
                GradeLevel = employee.Grade,
                StatusEmployee = employee.StatusEmployee,
                Email = employee.Email,             // Mengonversi Email dari Employee ke EmployeeDto.
                PhoneNumber = employee.PhoneNumber,  // Mengonversi Nomor Telepon dari Employee ke EmployeeDto.
                CompanyGuid = employee.CompanyGuid  // Mengonversi Nomor Telepon dari Employee ke EmployeeDto.
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
                Gender = dto.Gender,
                Foto = dto.Foto, // Mengonversi Jenis Kelamin dari EmployeeDto ke Employee.
                Grade = dto.GradeLevel,
                StatusEmployee = dto.StatusEmployee,
                Email = dto.Email,              
                PhoneNumber = dto.PhoneNumber,
                CompanyGuid = dto.CompanyGuid,// Mengonversi Nomor Telepon dari EmployeeDto ke Employee.
                ModifiedDate = DateTime.Now
            };
        }
    }
}
