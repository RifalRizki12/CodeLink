using API.Models;

namespace API.DTOs.Companies
{
    public class CreateCompanyDto
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }

        // Operator implisit untuk mengonversi CreateCompanyDto ke entitas Role.
        public static implicit operator Company(CreateCompanyDto createDto)
        {
            return new Company
            {
                Name = createDto.Name, // Mengisi properti Name dengan nilai dari CreateCompanyDto.
                Address = createDto.Address,
                Description = createDto.Description,
                CreatedDate = DateTime.Now, // Mengisi properti CreatedDate dengan tanggal dan waktu saat ini.
                ModifiedDate = DateTime.Now // Mengisi properti ModifiedDate dengan tanggal dan waktu saat ini.
            };
        }
    }
}
