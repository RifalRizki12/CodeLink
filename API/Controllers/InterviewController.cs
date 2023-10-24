using API.Contracts;
using API.Data;
using API.DTOs.Interviews;
using API.Models;
using API.Repositories;
using API.Utilities.Handler;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Principal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InterviewController : ControllerBase
    {
        private readonly IInterviewRepository _interviewRepository;
        private readonly IEmailHandler _emailHandler;
        private readonly ICompanyRepository _companyRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IRatingRepository _ratingRepository;


        public InterviewController(IInterviewRepository interviewRepository, IEmailHandler emailHandler,
            IEmployeeRepository employeeRepository, IAccountRepository accountRepository,
            IRatingRepository ratingRepository, ICompanyRepository companyRepository)
        {
            _interviewRepository = interviewRepository;
            _emailHandler = emailHandler;
            _employeeRepository = employeeRepository;
            _accountRepository = accountRepository;
            _ratingRepository = ratingRepository;
            _companyRepository = companyRepository;
        }
        [HttpPut("Announcement")]
        public IActionResult Announcement(AnnouncmentDto announcment)
        {
            try
            {
                // Dapatkan data interview berdasarkan Guid.
                // var existInterview = _interviewRepository.GetByGuid(announcment.Guid);

                var company = _companyRepository.GetCompany(announcment.OwnerGuid);

                var entity = _interviewRepository.GetByGuid(announcment.Guid);
                if (entity is null)
                {
                    // Mengembalikan respons Not Found jika Interview dengan GUID tertentu tidak ditemukan.
                    return NotFound(new ResponseErrorHandler
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = HttpStatusCode.NotFound.ToString(),
                        Message = "Interview with Specific GUID Not Found"
                    });
                }

                // Menyalin nilai CreatedDate dari entitas yang ada ke entitas yang akan diperbarui.
                Interview toUpdate = announcment;
                toUpdate.CreatedDate = entity.CreatedDate;
                toUpdate.Name = entity.Name;
                toUpdate.Date = entity.Date;

                // Memanggil metode Update dari _interviewRepository untuk memperbarui data Interview.
                var result = _interviewRepository.Update(toUpdate);

                Rating rating = announcment;
                rating.Guid = toUpdate.Guid;
                rating.Feedback = announcment.FeedBack;
                var resultUpdate = _interviewRepository.Update(toUpdate);

                // Memeriksa apakah penciptaan data berhasil atau gagal.
                if ((result == null) && (resultUpdate == null))
                {
                    // Mengembalikan respons BadRequest jika gagal membuat data Interview.
                    return BadRequest("Failed to create data");
                }

                // Mengirim email ke employee dengan GUID tertentu
                var specificEmployee = _employeeRepository.GetByGuid(announcment.EmployeeGuid);
                if (specificEmployee != null)
                {

                    _emailHandler.Send("Interview Schedule",
                        $"Terimakasih atas partisipasinya telah mengikuti proses {announcment.Name} pada\n" +
                        $"Date                  : {announcment.Date}\n" +
                        $"kami nyatakan anda    : {announcment.Description}\n" +
                        $"Start Contract        : {announcment.StartContract} \n" +
                        $"End Contract          : {announcment.EndContract}\n" +
                        $"Company Name          : {company.Name}\n" +
                        $"{announcment.FeedBack}", specificEmployee.Email);
                }

                // Mengembalikan data yang berhasil dibuat dalam respons OK.
                return Ok(new ResponseOKHandler<string>("Announcement sent successfully"));

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


        // GET api/interview
        [HttpGet]
        public IActionResult GetAll()
        {
            // Memanggil metode GetAll dari _interviewRepository untuk mendapatkan semua data Interview.
            var result = _interviewRepository.GetAll();

            // Memeriksa apakah hasil query tidak mengandung data.
            if (!result.Any())
            {
                // Mengembalikan respons Not Found jika tidak ada data Interview.
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data Interview Not Found"
                });
            }

            // Mengonversi hasil query ke objek DTO (Data Transfer Object) menggunakan Select.
            var data = result.Select(x => (InterviewDto)x);

            // Mengembalikan data yang ditemukan dalam respons OK.
            return Ok(new ResponseOKHandler<IEnumerable<InterviewDto>>(data));
        }

        // GET api/interview/{guid}
        [HttpGet("{guid}")]
        public IActionResult GetByGuid(Guid guid)
        {
            // Memanggil metode GetByGuid dari _interviewRepository dengan parameter GUID.
            var result = _interviewRepository.GetByGuid(guid);

            // Memeriksa apakah hasil query tidak ditemukan (null).
            if (result is null)
            {
                // Mengembalikan respons Not Found jika data Interview dengan GUID tertentu tidak ditemukan.
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Interview Data with Specific GUID Not Found"
                });
            }

            // Mengonversi hasil query ke objek DTO (Data Transfer Object).
            return Ok(new ResponseOKHandler<InterviewDto>((InterviewDto)result));
        }

        // POST api/interview
        [HttpPost]
        public IActionResult InterviewClient(CreateInterviewDto interviewDto)
        {
            try
            {
                // Mengonversi DTO CreateInterviewDto menjadi objek Interview.
                Interview toCreate = interviewDto;

                // Memanggil metode Create dari _interviewRepository untuk membuat data Interview baru.
                var result = _interviewRepository.Create(toCreate);

                Rating rating = interviewDto;
                rating.Guid = result.Guid;
                Rating createRating = _ratingRepository.Create(rating);

                // Memeriksa apakah penciptaan data berhasil atau gagal.
                if (result is null)
                {
                    // Mengembalikan respons BadRequest jika gagal membuat data Interview.
                    return BadRequest("Failed to create data");
                }

                // Get employee dengan role "admin"
                var adminEmployee = _employeeRepository.GetAdminEmployee();

                if (adminEmployee != null)
                {
                    _emailHandler.Send("Interview Schedule", $"Your Schedule {interviewDto.Name} at {interviewDto.Date}", adminEmployee.Email);
                }

                // Mengirim email ke employee dengan GUID tertentu
                var specificEmployee = _employeeRepository.GetByGuid(interviewDto.EmployeeGuid); // Ganti dengan metode yang sesuai
                if (specificEmployee != null)
                {
                    _emailHandler.Send("Interview Schedule", $"Your Schedule {interviewDto.Name} at {interviewDto.Date}", specificEmployee.Email);
                }

                // Mengembalikan data yang berhasil dibuat dalam respons OK.
                return Ok(new ResponseOKHandler<InterviewDto>((InterviewDto)result));
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

        // PUT api/interview
        [HttpPut]
        public IActionResult Update(InterviewDto interviewDto)
        {
            try
            {
                // Memeriksa apakah entitas Interview yang akan diperbarui ada dalam database.
                var entity = _interviewRepository.GetByGuid(interviewDto.Guid);
                if (entity is null)
                {
                    // Mengembalikan respons Not Found jika Interview dengan GUID tertentu tidak ditemukan.
                    return NotFound(new ResponseErrorHandler
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = HttpStatusCode.NotFound.ToString(),
                        Message = "Interview with Specific GUID Not Found"
                    });
                }

                // Menyalin nilai CreatedDate dari entitas yang ada ke entitas yang akan diperbarui.
                Interview toUpdate = interviewDto;
                toUpdate.CreatedDate = entity.CreatedDate;

                // Memanggil metode Update dari _interviewRepository untuk memperbarui data Interview.
                var result = _interviewRepository.Update(toUpdate);

                // Memeriksa apakah pembaruan data berhasil atau gagal.
                if (!result)
                {
                    // Mengembalikan respons BadRequest jika gagal memperbarui data Interview.
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

        // DELETE api/interview/{guid}
        [HttpDelete("{guid}")]
        public IActionResult Delete(Guid guid)
        {
            try
            {
                // Mengambil data interview berdasarkan GUID
                var entity = _interviewRepository.GetByGuid(guid);
                // Jika data interview tidak ditemukan
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

                // Menghapus data interview dari repository
                _interviewRepository.Delete(entity);

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
