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

            if (!result.Any())
            {
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data Role Not Found"
                });
            }

            // Mengonversi hasil query ke objek DTO (Data Transfer Object) menggunakan Select.
            var data = result.Select(x => (RoleDto)x);

            return Ok(new ResponseOKHandler<IEnumerable<RoleDto>>(data));
        }

        // GET api/role/{guid}
        [HttpGet("{guid}")]
        public IActionResult GetByGuid(Guid guid)
        {
            // Memanggil metode GetByGuid dari _roleRepository dengan parameter GUID.
            var result = _roleRepository.GetByGuid(guid);

            if (result is null)
            {
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Role Data with Specific GUID Not Found"
                });
            }

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

                if (result is null)
                {
                    return BadRequest("Failed to create data");
                }

                return Ok(new ResponseOKHandler<RoleDto>((RoleDto)result));
            }
            catch (ExceptionHandler ex)
            {
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

                if (!result)
                {
                    return BadRequest("Failed to update data");
                }

                return Ok("Data Has Been Updated");
            }
            catch (ExceptionHandler ex)
            {
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
                // Mengambil data role berdasarkan GUID
                var entity = _roleRepository.GetByGuid(guid);
                if (entity is null)
                {
                    return NotFound(new ResponseErrorHandler
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = HttpStatusCode.NotFound.ToString(),
                        Message = "Data Not Found"
                    });
                }

                // Menghapus data role dari repository
                _roleRepository.Delete(entity);

                // Mengembalikan pesan bahwa data telah dihapus dengan kode 200
                return Ok(new ResponseOKHandler<string>("Data Deleted"));
            }
            catch (ExceptionHandler ex)
            {
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
