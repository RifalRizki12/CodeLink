using API.Contracts;
using API.DTOs.Companies;
using API.DTOs.Skills;
using API.Models;
using API.Repositories;
using API.Utilities.Handler;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyRepository _companyRepository;

        public CompanyController(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        // GET api/company
        [HttpGet]
        public IActionResult GetAll()
        {
            // Memanggil metode GetAll dari _companyRepository untuk mendapatkan semua data company.
            var result = _companyRepository.GetAll();

            // Memeriksa apakah hasil query tidak mengandung data.
            if (!result.Any())
            {
                // Mengembalikan respons Not Found jika tidak ada data company.
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data Company Not Found"
                });
            }

            // Mengonversi hasil query ke objek DTO (Data Transfer Object) menggunakan Select.
            var data = result.Select(x => (CompanyDto)x);

            // Mengembalikan data yang ditemukan dalam respons OK.
            return Ok(new ResponseOKHandler<IEnumerable<CompanyDto>>(data));
        }

        // GET api/company/{guid}
        [HttpGet("{guid}")]
        public IActionResult GetByGuid(Guid guid)
        {
            // Memanggil metode GetByGuid dari _companyRepository dengan parameter GUID.
            var result = _companyRepository.GetByGuid(guid);

            // Memeriksa apakah hasil query tidak ditemukan (null).
            if (result is null)
            {
                // Mengembalikan respons Not Found jika data company dengan GUID tertentu tidak ditemukan.
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Company Data with Specific GUID Not Found"
                });
            }

            // Mengonversi hasil query ke objek DTO (Data Transfer Object).
            return Ok(new ResponseOKHandler<CompanyDto>((CompanyDto)result));
        }

        // PUT api/company
        [HttpPut]
        public IActionResult Update(CompanyDto companyDto)
        {
            try
            {
                // Memeriksa apakah entitas company yang akan diperbarui ada dalam database.
                var entity = _companyRepository.GetByGuid(companyDto.Guid);
                if (entity is null)
                {
                    // Mengembalikan respons Not Found jika company dengan GUID tertentu tidak ditemukan.
                    return NotFound(new ResponseErrorHandler
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = HttpStatusCode.NotFound.ToString(),
                        Message = "Company with Specific GUID Not Found"
                    });
                }

                // Menyalin nilai CreatedDate dari entitas yang ada ke entitas yang akan diperbarui.
                Company toUpdate = companyDto;
                toUpdate.CreatedDate = entity.CreatedDate;

                // Memanggil metode Update dari _companyRepository untuk memperbarui data company.
                _companyRepository.Update(toUpdate);


                // Mengembalikan pesan sukses dalam respons OK.
                return Ok(new ResponseOKHandler<string>("Data Has Been Deleted"));
            }
            catch (ExceptionHandler ex)
            {
                // Mengembalikan respons server error jika terjadi kesalahan dalam proses.
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorHandler
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Failed to update data",
                    Error = ex.Message
                });
            }
        }
    }
}
