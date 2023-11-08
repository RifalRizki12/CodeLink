using API.Contracts;
using API.DTOs.Roles;
using API.DTOs.Skills;
using API.Models;
using API.Utilities.Handler;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SkillController : ControllerBase
    {
        private readonly ISkillRepository _skillRepository;

        public SkillController(ISkillRepository skillRepository)
        {
            _skillRepository = skillRepository;
        }

        // GET api/skill
        [HttpGet]
        public IActionResult GetAll()
        {
            // Memanggil metode GetAll dari _skillRepository untuk mendapatkan semua data skill.
            var result = _skillRepository.GetAll();


            if (!result.Any())
            {

                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data Skill Not Found"
                });
            }

            // Mengonversi hasil query ke objek DTO (Data Transfer Object) menggunakan Select.
            var data = result.Select(x => (SkillDto)x);


            return Ok(new ResponseOKHandler<IEnumerable<SkillDto>>(data));
        }

        // GET api/skill/{guid}
        [HttpGet("{guid}")]
        public IActionResult GetByGuid(Guid guid)
        {
            // Memanggil metode GetByGuid dari _skillRepository dengan parameter GUID.
            var result = _skillRepository.GetByGuid(guid);


            if (result is null)
            {
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Skill Data with Specific GUID Not Found"
                });
            }

            return Ok(new ResponseOKHandler<SkillDto>((SkillDto)result));
        }

        // POST api/skill
        [HttpPost]
        public IActionResult Create(CreateSkillDto skillDto)
        {
            try
            {
                // Memanggil metode Create dari _skillRepository untuk membuat data skill baru.
                var result = _skillRepository.Create(skillDto);

                return Ok(new ResponseOKHandler<SkillDto>((SkillDto)result));
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

        // PUT api/skill
        [HttpPut]
        public IActionResult Update(SkillDto skillDto)
        {
            try
            {
                // Memeriksa apakah entitas skill yang akan diperbarui ada dalam database.
                var entity = _skillRepository.GetByGuid(skillDto.Guid);
                if (entity is null)
                {
                    return NotFound(new ResponseErrorHandler
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = HttpStatusCode.NotFound.ToString(),
                        Message = "Skill with Specific GUID Not Found"
                    });
                }

                // Menyalin nilai CreatedDate dari entitas yang ada ke entitas yang akan diperbarui.
                Skill toUpdate = skillDto;
                toUpdate.CreatedDate = entity.CreatedDate;

                // Memanggil metode Update dari _skillRepository untuk memperbarui data skill.
               _skillRepository.Update(toUpdate);


                return Ok(new ResponseOKHandler<string>("Data Has Been Updated"));
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

        // DELETE api/skill/{guid}
        [HttpDelete("{guid}")]
        public IActionResult Delete(Guid guid)
        {
            try
            {
                // Memanggil metode GetByGuid dari _skillRepository untuk mendapatkan entitas yang akan dihapus.
                var entity = _skillRepository.GetByGuid(guid);

                if (entity is null)
                {
                    return NotFound(new ResponseErrorHandler
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = HttpStatusCode.NotFound.ToString(),
                        Message = "Skill Not Found"
                    });
                }

                // Memanggil metode Delete dari _skillRepository untuk menghapus data skill.
               _skillRepository.Delete(entity);


                return Ok(new ResponseOKHandler<string>("Data Has Been Deleted"));
            }
            catch (ExceptionHandler ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorHandler
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Failed to delete skill",
                    Error = ex.Message
                });
            }
        }
    }
}
