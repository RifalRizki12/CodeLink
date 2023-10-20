using API.Contracts;
using API.DTOs.Roles;
using API.Models;
using API.Utilities.Handler;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleRepository _roleRepository;

        public RoleController(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        // GET api/role
        [HttpGet]
        public IActionResult GetAll()
        {
            // Memanggil metode GetAll dari _roleRepository untuk mendapatkan semua data Role.
            var result = _roleRepository.GetAll();

            // Memeriksa apakah hasil query tidak mengandung data.
            if (!result.Any())
            {
                // Mengembalikan respons Not Found jika tidak ada data Role.
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data Role Not Found"
                });
            }

            // Mengonversi hasil query ke objek DTO (Data Transfer Object) menggunakan Select.
            var data = result.Select(x => (RoleDto)x);

            // Mengembalikan data yang ditemukan dalam respons OK.
            return Ok(new ResponseOKHandler<IEnumerable<RoleDto>>(data));
        }

        // GET api/role/{guid}
        [HttpGet("{guid}")]
        public IActionResult GetByGuid(Guid guid)
        {
            // Memanggil metode GetByGuid dari _roleRepository dengan parameter GUID.
            var result = _roleRepository.GetByGuid(guid);

            // Memeriksa apakah hasil query tidak ditemukan (null).
            if (result is null)
            {
                // Mengembalikan respons Not Found jika data Role dengan GUID tertentu tidak ditemukan.
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Role Data with Specific GUID Not Found"
                });
            }

            // Mengonversi hasil query ke objek DTO (Data Transfer Object).
            return Ok(new ResponseOKHandler<RoleDto>((RoleDto)result));
        }

        // POST api/role
        [HttpPost]
        public IActionResult Create(CreateRoleDto roleDto)
        {
            try
            {
                // Mengonversi DTO CreateRoleDto menjadi objek Role.
                Role toCreate = roleDto;

                // Memanggil metode Create dari _roleRepository untuk membuat data Role baru.
                var result = _roleRepository.Create(toCreate);

                // Memeriksa apakah penciptaan data berhasil atau gagal.
                if (result is null)
                {
                    // Mengembalikan respons BadRequest jika gagal membuat data Role.
                    return BadRequest("Failed to create data");
                }

                // Mengembalikan data yang berhasil dibuat dalam respons OK.
                return Ok(new ResponseOKHandler<RoleDto>((RoleDto)result));
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

        // PUT api/role
        [HttpPut]
        public IActionResult Update(RoleDto roleDto)
        {
            try
            {
                // Memeriksa apakah entitas Role yang akan diperbarui ada dalam database.
                var entity = _roleRepository.GetByGuid(roleDto.Guid);
                if (entity is null)
                {
                    // Mengembalikan respons Not Found jika Role dengan GUID tertentu tidak ditemukan.
                    return NotFound(new ResponseErrorHandler
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = HttpStatusCode.NotFound.ToString(),
                        Message = "Role with Specific GUID Not Found"
                    });
                }

                // Menyalin nilai CreatedDate dari entitas yang ada ke entitas yang akan diperbarui.
                Role toUpdate = roleDto;
                toUpdate.CreatedDate = entity.CreatedDate;

                // Memanggil metode Update dari _roleRepository untuk memperbarui data Role.
                var result = _roleRepository.Update(toUpdate);

                // Memeriksa apakah pembaruan data berhasil atau gagal.
                if (!result)
                {
                    // Mengembalikan respons BadRequest jika gagal memperbarui data Role.
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

        // DELETE api/role/{guid}
        [HttpDelete("{guid}")]
        public IActionResult Delete(Guid guid)
        {
            try
            {
                // Memanggil metode GetByGuid dari _roleRepository untuk mendapatkan entitas yang akan dihapus.
                var existingRole = _roleRepository.GetByGuid(guid);

                // Memeriksa apakah entitas yang akan dihapus ada dalam database.
                if (existingRole is null)
                {
                    // Mengembalikan respons Not Found jika Role tidak ditemukan.
                    return NotFound(new ResponseErrorHandler
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = HttpStatusCode.NotFound.ToString(),
                        Message = "Role Not Found"
                    });
                }

                // Memanggil metode Delete dari _roleRepository untuk menghapus data Role.
                var deleted = _roleRepository.Delete(existingRole);

                // Memeriksa apakah penghapusan data berhasil atau gagal.
                if (!deleted)
                {
                    // Mengembalikan respons BadRequest jika gagal menghapus Role.
                    return BadRequest("Failed to delete role");
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
                    Message = "Failed to delete role",
                    Error = ex.Message
                });
            }
        }
    }
}
