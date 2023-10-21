using API.Contracts;
using API.DTOs.Ratings;
using API.Models;
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

        public RatingController(IRatingRepository ratingRepository)
        {
            _ratingRepository = ratingRepository;
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
                // Memanggil metode GetByGuid dari _ratingRepository untuk mendapatkan entitas yang akan dihapus.
                var existingRating = _ratingRepository.GetByGuid(guid);

                // Memeriksa apakah entitas yang akan dihapus ada dalam database.
                if (existingRating is null)
                {
                    // Mengembalikan respons Not Found jika Rating tidak ditemukan.
                    return NotFound(new ResponseErrorHandler
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = HttpStatusCode.NotFound.ToString(),
                        Message = "Rating Not Found"
                    });
                }

                // Memanggil metode Delete dari _ratingRepository untuk menghapus data Rating.
                var deleted = _ratingRepository.Delete(existingRating);

                // Memeriksa apakah penghapusan data berhasil atau gagal.
                if (!deleted)
                {
                    // Mengembalikan respons BadRequest jika gagal menghapus Rating.
                    return BadRequest("Failed to delete rating");
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
                    Message = "Failed to delete rating",
                    Error = ex.Message
                });
            }
        }
    }
}
