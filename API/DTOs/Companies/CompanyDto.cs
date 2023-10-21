using API.Models;

namespace API.DTOs.Companies
{
    public class CompanyDto
    {
        public Guid Guid { get; set; }
        public string Name { get; set; }
        public string Address {  get; set; }
        public string? Description { get; set; }

        // Operator eksplisit untuk mengonversi Company ke CompanyDto.
        public static explicit operator CompanyDto(Company company)
        {
            return new CompanyDto
            {
                Guid = company.Guid, // Mengisi properti Guid dengan nilai dari entitas Company.
                Name = company.Name,
                Address = company.Address,
                Description = company.Description,
            };
        }

        // Operator implisit untuk mengonversi CompanyDto ke Company.
        public static implicit operator Company(CompanyDto companyDto)
        {
            return new Company
            {
                Guid = companyDto.Guid,       // Mengisi properti Guid dengan nilai dari CompanyDto.
                Name = companyDto.Name,       // Mengisi properti Name dengan nilai dari CompanyDto.
                Address = companyDto.Address,
                Description = companyDto.Description,
                ModifiedDate = DateTime.Now // Mengisi properti ModifiedDate dengan tanggal dan waktu saat ini.
            };
        }
    }
}
