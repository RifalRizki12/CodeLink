using API.Contracts;
using API.DTOs.ExperienceSkills;
using API.Models;
using API.Utilities.Handler;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurriculumVitaeController : ControllerBase
    {
        private readonly ICurriculumVitaeRepository _cvRepository;

        public CurriculumVitaeController(ICurriculumVitaeRepository cvRepository)
        {
            _cvRepository = cvRepository;
        }

        // GET api/experienceSkill
        [HttpGet]
        public IActionResult GetAll()
        {
            // Memanggil metode GetAll dari _cvRepository untuk mendapatkan semua data experienceSkill.
            var result = _cvRepository.GetAll();

            // Memeriksa apakah hasil query tidak mengandung data.
            if (!result.Any())
            {
                // Mengembalikan respons Not Found jika tidak ada data experienceSkill.
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data Curriculum Vitae Not Found"
                });
            }

            // Mengonversi hasil query ke objek DTO (Data Transfer Object) menggunakan Select.
            var data = result.Select(x => (CurriculumVitaeDto)x);

            // Mengembalikan data yang ditemukan dalam respons OK.
            return Ok(new ResponseOKHandler<IEnumerable<CurriculumVitaeDto>>(data));
        }

        // GET api/experienceSkill/{guid}
        [HttpGet("{guid}")]
        public IActionResult GetByGuid(Guid guid)
        {
            // Memanggil metode GetByGuid dari _cvRepository dengan parameter GUID.
            var result = _cvRepository.GetByGuid(guid);

            // Memeriksa apakah hasil query tidak ditemukan (null).
            if (result is null)
            {
                // Mengembalikan respons Not Found jika data ExperienceSkill dengan GUID tertentu tidak ditemukan.
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Curriculum Vitae Data with Specific GUID Not Found"
                });
            }

            // Mengonversi hasil query ke objek DTO (Data Transfer Object).
            return Ok(new ResponseOKHandler<CurriculumVitaeDto>((CurriculumVitaeDto)result));
        }

        // POST api/ExperienceSkill
        [HttpPost]
        public IActionResult Create(CreateCurriculumVitaeDto cvDto)
        {
            try
            {

                // Memanggil metode Create dari _cvRepository untuk membuat data ExperienceSkill baru.
                var result = _cvRepository.Create(cvDto);

                // Mengembalikan data yang berhasil dibuat dalam respons OK.
                return Ok(new ResponseOKHandler<CurriculumVitaeDto>((CurriculumVitaeDto)result));
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

        // PUT api/experienceSkill
        [HttpPut]
        public IActionResult Update(CurriculumVitaeDto expDto)
        {
            try
            {
                // Memeriksa apakah entitas ExperienceSkill yang akan diperbarui ada dalam database.
                var entity = _cvRepository.GetByGuid(expDto.Guid);
                if (entity is null)
                {
                    // Mengembalikan respons Not Found jika experienceSkill dengan GUID tertentu tidak ditemukan.
                    return NotFound(new ResponseErrorHandler
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = HttpStatusCode.NotFound.ToString(),
                        Message = " Curriculum Vitae with Specific GUID Not Found"
                    });
                }

                // Menyalin nilai CreatedDate dari entitas yang ada ke entitas yang akan diperbarui.
                CurriculumVitae toUpdate = expDto;
                toUpdate.CreatedDate = entity.CreatedDate;

                // Memanggil metode Update dari _cvRepository untuk memperbarui data experienceSkill.
               _cvRepository.Update(expDto);


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

        // DELETE api/experienceSkill/{guid}
        [HttpDelete("{guid}")]
        public IActionResult Delete(Guid guid)
        {
            try
            {
                // Memanggil metode GetByGuid dari _cvRepository untuk mendapatkan entitas yang akan dihapus.
                var entity = _cvRepository.GetByGuid(guid);

                // Memeriksa apakah entitas yang akan dihapus ada dalam database.
                if (entity is null)
                {
                    // Mengembalikan respons Not Found jika experienceSkill tidak ditemukan.
                    return NotFound(new ResponseErrorHandler
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = HttpStatusCode.NotFound.ToString(),
                        Message = "Curriculum Vitae Not Found"
                    });
                }

                // Memanggil metode Delete dari _cvRepository untuk menghapus data ExperienceSkill.
               _cvRepository.Delete(entity);


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
                    Message = "Failed to delete Curriculum Vitae",
                    Error = ex.Message
                });
            }
        }
    }
}
