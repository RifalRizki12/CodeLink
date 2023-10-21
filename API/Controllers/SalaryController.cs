using API.Contracts;
using API.DTOs.Salaries;
using API.Models;
using API.Utilities.Handler;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalaryController : ControllerBase
    {
        private readonly ISalaryRepository _salaryRepository;

        public SalaryController(ISalaryRepository salaryRepository)
        {
            _salaryRepository = salaryRepository;
        }

        // GET api/salary
        [HttpGet]
        public IActionResult GetAll()
        {
            // Memanggil metode GetAll dari _salaryRepository untuk mendapatkan semua data Salary.
            var result = _salaryRepository.GetAll();

            // Memeriksa apakah hasil query tidak mengandung data.
            if (!result.Any())
            {
                // Mengembalikan respons Not Found jika tidak ada data Salary.
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data Salary Not Found"
                });
            }

            // Mengonversi hasil query ke objek DTO (Data Transfer Object) menggunakan Select.
            var data = result.Select(x => (SalaryDto)x);

            // Mengembalikan data yang ditemukan dalam respons OK.
            return Ok(new ResponseOKHandler<IEnumerable<SalaryDto>>(data));
        }

        // GET api/salary/{guid}
        [HttpGet("{guid}")]
        public IActionResult GetByGuid(Guid guid)
        {
            // Memanggil metode GetByGuid dari _salaryRepository dengan parameter GUID.
            var result = _salaryRepository.GetByGuid(guid);

            // Memeriksa apakah hasil query tidak ditemukan (null).
            if (result is null)
            {
                // Mengembalikan respons Not Found jika data Salary dengan GUID tertentu tidak ditemukan.
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Salary Data with Specific GUID Not Found"
                });
            }

            // Mengonversi hasil query ke objek DTO (Data Transfer Object).
            return Ok(new ResponseOKHandler<SalaryDto>((SalaryDto)result));
        }

        // POST api/salary
        [HttpPost]
        public IActionResult Create(CreateSalaryDto salaryDto)
        {
            try
            {
                // Mengonversi DTO CreateSalaryDto menjadi objek Salary.
                Salary toCreate = salaryDto;

                // Memanggil metode Create dari _salaryRepository untuk membuat data Salary baru.
                var result = _salaryRepository.Create(toCreate);

                // Memeriksa apakah penciptaan data berhasil atau gagal.
                if (result is null)
                {
                    // Mengembalikan respons BadRequest jika gagal membuat data Salary.
                    return BadRequest("Failed to create data");
                }

                // Mengembalikan data yang berhasil dibuat dalam respons OK.
                return Ok(new ResponseOKHandler<SalaryDto>((SalaryDto)result));
            }
            catch (ExceptionHandler ex)
            {
                // Mengembalikan respons server error jika terjadi kesalahan dalam proses.
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorHandler
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Failed to create data",
                    Error = ex.Message
                });
            }
        }

        // PUT api/salary
        [HttpPut]
        public IActionResult Update(SalaryDto salaryDto)
        {
            try
            {
                // Memeriksa apakah entitas Salary yang akan diperbarui ada dalam database.
                var entity = _salaryRepository.GetByGuid(salaryDto.Guid);
                if (entity is null)
                {
                    // Mengembalikan respons Not Found jika Salary dengan GUID tertentu tidak ditemukan.
                    return NotFound(new ResponseErrorHandler
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = HttpStatusCode.NotFound.ToString(),
                        Message = "Salary with Specific GUID Not Found"
                    });
                }

                // Menyalin nilai CreatedDate dari entitas yang ada ke entitas yang akan diperbarui.
                Salary toUpdate = salaryDto;
                toUpdate.CreatedDate = entity.CreatedDate;

                // Memanggil metode Update dari _salaryRepository untuk memperbarui data Salary.
                var result = _salaryRepository.Update(toUpdate);

                // Memeriksa apakah pembaruan data berhasil atau gagal.
                if (!result)
                {
                    // Mengembalikan respons BadRequest jika gagal memperbarui data Salary.
                    return BadRequest("Failed to update data");
                }

                // Mengembalikan pesan sukses dalam respons OK.
                return Ok("Data Has Been Updated");
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

        // DELETE api/salary/{guid}
        [HttpDelete("{guid}")]
        public IActionResult Delete(Guid guid)
        {
            try
            {
                // Memanggil metode GetByGuid dari _salaryRepository untuk mendapatkan entitas yang akan dihapus.
                var existingSalary = _salaryRepository.GetByGuid(guid);

                // Memeriksa apakah entitas yang akan dihapus ada dalam database.
                if (existingSalary is null)
                {
                    // Mengembalikan respons Not Found jika Salary tidak ditemukan.
                    return NotFound(new ResponseErrorHandler
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = HttpStatusCode.NotFound.ToString(),
                        Message = "Salary Not Found"
                    });
                }

                // Memanggil metode Delete dari _salaryRepository untuk menghapus data Salary.
                var deleted = _salaryRepository.Delete(existingSalary);

                // Memeriksa apakah penghapusan data berhasil atau gagal.
                if (!deleted)
                {
                    // Mengembalikan respons BadRequest jika gagal menghapus Salary.
                    return BadRequest("Failed to delete salary");
                }

                // Mengembalikan kode status 204 (No Content) untuk sukses penghapusan tanpa respons.
                return NoContent();
            }
            catch (ExceptionHandler ex)
            {
                // Mengembalikan respons server error jika terjadi kesalahan dalam proses.
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorHandler
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Failed to delete salary",
                    Error = ex.Message
                });
            }
        }
    }
}
