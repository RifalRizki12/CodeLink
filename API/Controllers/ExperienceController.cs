using API.Contracts;
using API.DTOs.Experiences;
using API.Models;
using API.Utilities.Handler;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExperienceController : ControllerBase
    {
        private readonly IExperienceRepository _experienceRepository;

        public ExperienceController(IExperienceRepository experienceRepository)
        {
            _experienceRepository = experienceRepository;
        }

        // GET api/experience
        [HttpGet]
        public IActionResult GetAll()
        {
            // Memanggil metode GetAll dari _experienceRepository untuk mendapatkan semua data experience.
            var result = _experienceRepository.GetAll();

            // Memeriksa apakah hasil query tidak mengandung data.
            if (!result.Any())
            {
                // Mengembalikan respons Not Found jika tidak ada data experience.
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data Experience Not Found"
                });
            }

            // Mengonversi hasil query ke objek DTO (Data Transfer Object) menggunakan Select.
            var data = result.Select(x => (ExperienceDto)x);

            // Mengembalikan data yang ditemukan dalam respons OK.
            return Ok(new ResponseOKHandler<IEnumerable<ExperienceDto>>(data));
        }

        // GET api/experience/{guid}
        [HttpGet("{guid}")]
        public IActionResult GetByGuid(Guid guid)
        {
            // Memanggil metode GetByGuid dari _experienceRepository dengan parameter GUID.
            var result = _experienceRepository.GetByGuid(guid);

            // Memeriksa apakah hasil query tidak ditemukan (null).
            if (result is null)
            {
                // Mengembalikan respons Not Found jika data experience dengan GUID tertentu tidak ditemukan.
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Experience Data with Specific GUID Not Found"
                });
            }

            // Mengonversi hasil query ke objek DTO (Data Transfer Object).
            return Ok(new ResponseOKHandler<ExperienceDto>((ExperienceDto)result));
        }

        // POST api/experience
        [HttpPost]
        public IActionResult Create(CreateExperienceDto experienceDto)
        {
            try
            {

                // Memanggil metode Create dari _experienceRepository untuk membuat data experience baru.
                var result = _experienceRepository.Create(experienceDto);

                // Mengembalikan data yang berhasil dibuat dalam respons OK.
                return Ok(new ResponseOKHandler<ExperienceDto>((ExperienceDto)result));
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

        // PUT api/experience
        [HttpPut]
        public IActionResult Update(ExperienceDto expDto)
        {
            try
            {
                // Memeriksa apakah entitas experience yang akan diperbarui ada dalam database.
                var entity = _experienceRepository.GetByGuid(expDto.Guid);
                if (entity is null)
                {
                    // Mengembalikan respons Not Found jika experience dengan GUID tertentu tidak ditemukan.
                    return NotFound(new ResponseErrorHandler
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = HttpStatusCode.NotFound.ToString(),
                        Message = " Experience with Specific GUID Not Found"
                    });
                }

                // Menyalin nilai CreatedDate dari entitas yang ada ke entitas yang akan diperbarui.
                Experience toUpdate = expDto;
                toUpdate.CreatedDate = entity.CreatedDate;

                // Memanggil metode Update dari _experienceRepository untuk memperbarui data experience.
               _experienceRepository.Update(expDto);


                // Mengembalikan pesan sukses dalam respons OK.
                return Ok(new ResponseOKHandler<string>("Data Has Been Updated"));
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

        // DELETE api/experience/{guid}
        [HttpDelete("{guid}")]
        public IActionResult Delete(Guid guid)
        {
            try
            {
                // Memanggil metode GetByGuid dari _experienceRepository untuk mendapatkan entitas yang akan dihapus.
                var entity = _experienceRepository.GetByGuid(guid);

                // Memeriksa apakah entitas yang akan dihapus ada dalam database.
                if (entity is null)
                {
                    // Mengembalikan respons Not Found jika experience tidak ditemukan.
                    return NotFound(new ResponseErrorHandler
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = HttpStatusCode.NotFound.ToString(),
                        Message = "Experience Not Found"
                    });
                }

                // Memanggil metode Delete dari _experienceRepository untuk menghapus data experience.
               _experienceRepository.Delete(entity);


                // Mengembalikan kode status 204 (No Content) untuk sukses penghapusan tanpa respons.
                return Ok(new ResponseOKHandler<string>("Data Has Been Deleted"));
            }
            catch (ExceptionHandler ex)
            {
                // Mengembalikan respons server error jika terjadi kesalahan dalam proses.
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorHandler
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Failed to delete experience",
                    Error = ex.Message
                });
            }
        }
    }
}
