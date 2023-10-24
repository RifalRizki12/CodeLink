using API.Contracts;
using API.DTOs.Ratings;
using API.Models;
using API.Repositories;
using API.Utilities.Handler;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RatingController : ControllerBase
    {
        private readonly IRatingRepository _ratingRepository;
        private readonly IInterviewRepository _interviewRepository;

        public RatingController(IRatingRepository ratingRepository, IInterviewRepository interviewRepository)
        {
            _ratingRepository = ratingRepository;
            _interviewRepository = interviewRepository;
        }

        // GET api/rating
        [HttpGet]
        public IActionResult GetAll()
        {
            // Memanggil metode GetAll dari _ratingRepository untuk mendapatkan semua data Rating.
            var result = _ratingRepository.GetAll();

            // Memeriksa apakah hasil query tidak mengandung data.
            if (!result.Any())
            {
                // Mengembalikan respons Not Found jika tidak ada data Rating.
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data Rating Not Found"
                });
            }

            // Mengonversi hasil query ke objek DTO (Data Transfer Object) menggunakan Select.
            var data = result.Select(x => (RatingDto)x);

            // Mengembalikan data yang ditemukan dalam respons OK.
            return Ok(new ResponseOKHandler<IEnumerable<RatingDto>>(data));
        }

        // GET api/rating/{guid}
        [HttpGet("{guid}")]
        public IActionResult GetByGuid(Guid guid)
        {
            // Memanggil metode GetByGuid dari _ratingRepository dengan parameter GUID.
            var result = _ratingRepository.GetByGuid(guid);

            // Memeriksa apakah hasil query tidak ditemukan (null).
            if (result is null)
            {
                // Mengembalikan respons Not Found jika data Rating dengan GUID tertentu tidak ditemukan.
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Rating Data with Specific GUID Not Found"
                });
            }

            // Mengonversi hasil query ke objek DTO (Data Transfer Object).
            return Ok(new ResponseOKHandler<RatingDto>((RatingDto)result));
        }

        [HttpGet("average-rating/employee/{employeeGuid}")]
        public IActionResult GetAverageRatingByEmployee(Guid employeeGuid)
        {
            double? averageRating = _ratingRepository.GetAverageRatingByEmployee(employeeGuid);

            if (!averageRating.HasValue)
            {
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "No ratings found for the specified Employee"
                });
            }

            return Ok(new ResponseOKHandler<double>(averageRating.Value));
        }





        // POST api/rating
        [HttpPost]
        public IActionResult Create(CreateRatingDto ratingDto)
        {
            try
            {
                // Mengonversi DTO CreateRatingDto menjadi objek Rating.
                Rating toCreate = ratingDto;

                // Memanggil metode Create dari _ratingRepository untuk membuat data Rating baru.
                var result = _ratingRepository.Create(toCreate);

                // Memeriksa apakah penciptaan data berhasil atau gagal.
                if (result is null)
                {
                    // Mengembalikan respons BadRequest jika gagal membuat data Rating.
                    return BadRequest("Failed to create data");
                }

                // Mengembalikan data yang berhasil dibuat dalam respons OK.
                return Ok(new ResponseOKHandler<RatingDto>((RatingDto)result));
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

        // PUT api/rating
        [HttpPut]
        public IActionResult Update(RatingDto ratingDto)
        {
            try
            {
                // Memeriksa apakah entitas Rating yang akan diperbarui ada dalam database.
                var entity = _ratingRepository.GetByGuid(ratingDto.Guid);
                if (entity is null)
                {
                    // Mengembalikan respons Not Found jika Rating dengan GUID tertentu tidak ditemukan.
                    return NotFound(new ResponseErrorHandler
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = HttpStatusCode.NotFound.ToString(),
                        Message = "Rating with Specific GUID Not Found"
                    });
                }

                // Menyalin nilai CreatedDate dari entitas yang ada ke entitas yang akan diperbarui.
                Rating toUpdate = ratingDto;
                toUpdate.CreatedDate = entity.CreatedDate;

                // Memanggil metode Update dari _ratingRepository untuk memperbarui data Rating.
                var result = _ratingRepository.Update(toUpdate);

                // Memeriksa apakah pembaruan data berhasil atau gagal.
                if (!result)
                {
                    // Mengembalikan respons BadRequest jika gagal memperbarui data Rating.
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

        // DELETE api/rating/{guid}
        [HttpDelete("{guid}")]
        public IActionResult Delete(Guid guid)
        {
            try
            {
                // Mengambil data rating berdasarkan GUID
                var entity = _ratingRepository.GetByGuid(guid);
                // Jika data rating tidak ditemukan
                if (entity is null)
                {
                    // Mengembalikan respon error dengan kode 404
                    return NotFound(new ResponseErrorHandler
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = HttpStatusCode.NotFound.ToString(),
                        Message = "Data Not Found"
                    });
                }

                // Menghapus data rating dari repository
                _ratingRepository.Delete(entity);

                // Mengembalikan pesan bahwa data telah dihapus dengan kode 200
                return Ok(new ResponseOKHandler<string>("Data Deleted"));
            }
            catch (ExceptionHandler ex)
            {
                // Jika terjadi error saat penghapusan, mengembalikan respon error dengan kode 500
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorHandler
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Failed to create data",
                    Error = ex.Message
                });
            }
        }
    }
}
