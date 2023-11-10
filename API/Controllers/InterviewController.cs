using API.Contracts;
using API.DTOs.Employees;
using API.DTOs.Interviews;
using API.Models;
using API.Utilities.Enums;
using API.Utilities.Handler;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InterviewController : ControllerBase
{
    private readonly IInterviewRepository _interviewRepository;
    private readonly IEmailHandler _emailHandler;
    private readonly ICompanyRepository _companyRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IRatingRepository _ratingRepository;


    public InterviewController(IInterviewRepository interviewRepository, IEmailHandler emailHandler,
        IEmployeeRepository employeeRepository, IAccountRepository accountRepository,
        IRatingRepository ratingRepository, ICompanyRepository companyRepository)
    {
        _interviewRepository = interviewRepository;
        _emailHandler = emailHandler;
        _employeeRepository = employeeRepository;
        _ratingRepository = ratingRepository;
        _companyRepository = companyRepository;
    }

    [HttpPut("ScheduleInterview")]
    public IActionResult ScheduleInterview(ScheduleInterviewDto schedule)
    {
        try
        {
            // Dapatkan data interview berdasarkan Guid.
            // var existInterview = _interviewRepository.GetByGuid(announcment.Guid);

            var company = _companyRepository.GetCompany(schedule.OwnerGuid);

            var entity = _interviewRepository.GetByGuid(schedule.Guid);
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
            Interview toUpdate = schedule;
            toUpdate.CreatedDate = entity.CreatedDate;
            toUpdate.Name = entity.Name;
            toUpdate.Date = entity.Date;

            // Memanggil metode Update dari _interviewRepository untuk memperbarui data Interview.
            var result = _interviewRepository.Update(toUpdate);

            // Memeriksa apakah penciptaan data berhasil atau gagal.
            if ((result == null))
            {
                // Mengembalikan respons BadRequest jika gagal membuat data Interview.
                return BadRequest("Failed to create data");
            }


            string emailTemplatePath = "utilities/TemplateEmail/ScheduleDetails.html"; // Sesuaikan path Anda.
            string emailTemplate = System.IO.File.ReadAllText(emailTemplatePath);

            // mengganti variabel dalam template dengan data yang sesuai
            emailTemplate = emailTemplate
                .Replace("{InterviewName}", toUpdate.Name)
                .Replace("{InterviewDate}", toUpdate.Date.ToString())
                .Replace("{InterviewType}", schedule.Type.ToString())
                .Replace("{InterviewLocation}", schedule.Location)
                .Replace("{InterviewRemarks}", schedule.Remarks)
                .Replace("{CompanyName}", company.Name);

            var specificEmployee = _employeeRepository.GetByGuid(schedule.EmployeeGuid);
            var employeeOwner = _employeeRepository.GetByGuid(schedule.OwnerGuid);

            if (specificEmployee != null)
            {
                // Hilangkan seluruh baris yang mengandung placeholder nama peserta
                string emailTemplateSpecificEmployee = emailTemplate
                    .Replace("<td>Nama Peserta</td>", "")
                    .Replace("<td id=\"peserta\">:</td>", "")
                    .Replace("<td>{SpecificEmployeeName}</td>", "")
                    .Replace("{fullName}", specificEmployee.FirstName + " " + specificEmployee.LastName); ;
                _emailHandler.Send("Interview Schedule Details", emailTemplateSpecificEmployee, specificEmployee.Email);
            }


            if (employeeOwner != null)
            {
                string emailTemplateOwner = emailTemplate
                    .Replace("{SpecificEmployeeName}", specificEmployee.FirstName + " " + specificEmployee.LastName)
                    .Replace("{fullName}", employeeOwner.FirstName + " " + employeeOwner.LastName);
                _emailHandler.Send("Interview Schedule Details", emailTemplateOwner, employeeOwner.Email);
            }


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

    private void SendEmailToBoth(string subject, string body, string employeeEmail, string adminEmail)
    {
        _emailHandler.Send(subject, body, employeeEmail);
        _emailHandler.Send(subject, body, adminEmail);
    }

    [HttpPut("Announcement")]
    public IActionResult Announcement(AnnouncmentDto announcment)
    {
        try
        {
            var dateNow = DateTime.Today;

            var company = _companyRepository.GetCompany(announcment.OwnerGuid);
            var entity = _interviewRepository.GetByGuid(announcment.Guid);
            var specificEmployee = _employeeRepository.GetByGuid(announcment.EmployeeGuid);
            var client = _employeeRepository.GetByGuid(announcment.OwnerGuid);
            var adminEmployee = _employeeRepository.GetAdminEmployee();


            if (entity is null || adminEmployee is null)
            {
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Interview with Specific GUID Not Found"
                });
            }
            //pake function harusnya ini untuk ngecek employeeid yg punya endContract di interview hari ini 


            Interview toUpdate = announcment;
            toUpdate.CreatedDate = entity.CreatedDate;
            toUpdate.Name = entity.Name;
            toUpdate.Date = entity.Date;
            toUpdate.Location = entity.Location;
            toUpdate.Type = entity.Type;
            toUpdate.Remarks = entity.Remarks;

            var result = _interviewRepository.Update(toUpdate);

            Rating rating = announcment;
            rating.Guid = toUpdate.Guid;
            rating.Feedback = announcment.FeedBack;
            rating.Rate = announcment.Rate;
            var resultUpdate = _ratingRepository.Update(rating);

            if ((result == null) && (resultUpdate == null))
            {
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Interview with Specific GUID Not Found"
                });
            }

            if (specificEmployee != null && rating.Rate == null)
            {
                string emailTemplatePath = "utilities/TemplateEmail/Announcement.html"; // Sesuaikan path tempat template.
                string emailTemplate = System.IO.File.ReadAllText(emailTemplatePath);


                if (entity.EndContract == null) // annaouncment lolos / tidak 
                {
                    // mengganti variabel dalam template dengan data yang sesuai
                    // Menggantikan div dengan id "contractTermination" dengan string kosong
                    emailTemplate = Regex.Replace(emailTemplate, "<div id=\"contractTermination\"[^>]*>.*?</div>", "", RegexOptions.Singleline)
                   .Replace("{InterviewName}", toUpdate.Name)
                   .Replace("{InterviewDate}", toUpdate.Date.ToString())
                   .Replace("{CompanyName}", company.Name)
                   .Replace("{EmployeeName}", specificEmployee.FirstName + " " + specificEmployee.LastName)
                   .Replace("{StatusIntervew}", announcment.StatusIntervew.ToString());

                    
                   
                    if (announcment.StatusIntervew == StatusIntervew.Lolos)
                    {
                        specificEmployee.StatusEmployee = (StatusEmployee)2;
                        specificEmployee.CompanyGuid = company.Guid;
                        _employeeRepository.Update(specificEmployee);


                        emailTemplate = emailTemplate.Replace("{StartContract}", announcment.StartContract.ToString())
                                           .Replace("{EndContract}", announcment.EndContract.ToString())
                                           .Replace("<tr>\r\n                <td>FeedBack</td>\r\n                <td>:</td>\r\n                <td>{feedback}</td>\r\n            </tr>", ""); ;
                        
                        string emailLolos = Regex.Replace(emailTemplate, "<p id=\"toAdmin\"[^>]*>.*?</p>", "", RegexOptions.Singleline)
                                         .Replace("{fullName}", specificEmployee.FirstName + " " + specificEmployee.LastName);


                        _emailHandler.Send("Announcement of interview results", emailLolos, specificEmployee.Email);

                        string adminLolos = Regex.Replace(emailTemplate, "<p id=\"toIdle\"[^>]*>.*?</p>", "", RegexOptions.Singleline)
                                             .Replace("{fullName}", adminEmployee.FirstName);

                        _emailHandler.Send("Announcement of interview results", adminLolos, adminEmployee.Email);
                    }

                    if (announcment.StatusIntervew == StatusIntervew.TidakLolos)
                    {
                        emailTemplate = emailTemplate.Replace("{feedback}", announcment.FeedBack)
                                                     .Replace("<tr>\r\n                <td>Start Contract</td>\r\n                <td>:</td>\r\n                <td>{StartContract}</td>\r\n            </tr>", "")
                                                     .Replace("<tr>\r\n                <td>End Contract</td>\r\n                <td>:</td>\r\n                <td>{EndContract}</td>\r\n            </tr>", "")
;
                        string tidakLolos = Regex.Replace(emailTemplate, "<p id=\"toAdmin\"[^>]*>.*?</p>", "", RegexOptions.Singleline)
                                                      .Replace("{fullName}", specificEmployee.FirstName + " " + specificEmployee.LastName);

                        _emailHandler.Send("Announcement of interview results", tidakLolos, specificEmployee.Email);

                        string adminTdkLolos = Regex.Replace(emailTemplate, "<p id=\"toIdle\"[^>]*>.*?</p>", "", RegexOptions.Singleline)
                                                 .Replace("{fullName}", adminEmployee.FirstName);

                        _emailHandler.Send("Announcement of interview results", adminTdkLolos, adminEmployee.Email);

                    }
                }
                //ini untuk contract termination
                if (announcment.EndContract != null && announcment.EndContract.Value.Date == dateNow.Date)
                {
                    emailTemplate = Regex.Replace(emailTemplate, "<div id=\"Lolos\"[^>]*>.*?</div>", "", RegexOptions.Singleline)
                        .Replace("{Remarks}", announcment.Remarks)
                        .Replace("{companyName}", company.Name)
                        .Replace("{EmployeeName}", specificEmployee.FirstName + " " + specificEmployee.LastName);

                    string emailEndcontract = emailTemplate.Replace("{fullName}", specificEmployee.FirstName + " " + specificEmployee.LastName);

                    _emailHandler.Send("Contract Termination", emailEndcontract, specificEmployee.Email);


                    string adminEndContract = emailTemplate.Replace("{fullName}", adminEmployee.FirstName);
                    _emailHandler.Send("Contract Termination", adminEndContract, adminEmployee.Email);

                    // _emailHandler.Send("Contract Termination", emailEndcontract, adminEmployee.Email);

                    specificEmployee.StatusEmployee = StatusEmployee.idle;
                    specificEmployee.CompanyGuid = null;
                    _employeeRepository.Update(specificEmployee);
                }
                //ini untuk finish contact
                if (entity.Guid != null && entity.EndContract?.Date == dateNow.Date)
                {
                    string subject = "Contract Finished";
                    string body = $"Kami mengucapkan terima kasih pada saudara/i yang telah bergabung bersama kami. " +
                                  $"Semoga di lain waktu kita bisa jumpa kembali. " +
                                  $"Salam Hormat, {company.Name}";

                    SendEmailToBoth(subject, body, specificEmployee.Email, adminEmployee.Email);

                    specificEmployee.StatusEmployee = StatusEmployee.idle;  // Gantikan 0 dengan enum yang sesuai
                    specificEmployee.CompanyGuid = null;
                    _employeeRepository.Update(specificEmployee);
                }


            }

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
        var interviews = _interviewRepository.GetAll();
        var employees = _employeeRepository.GetAll();
        var ratings = _ratingRepository.GetAll();

        // Memeriksa apakah hasil query tidak mengandung data.
        if (!interviews.Any())
        {
            // Mengembalikan respons Not Found jika tidak ada data Interview.
            return NotFound(new ResponseErrorHandler
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data Interview Not Found"
            });
        }
        var interviewDto = (from interview in interviews
                            join empInterviewer in employees on interview.OwnerGuid equals empInterviewer.Guid
                            join empIdle in employees on interview.EmployeeGuid equals empIdle.Guid
                            join rat in ratings on interview.Guid equals rat.Guid
                            select new InterviewDto
                            {
                                Guid = interview.Guid,
                                EmployeeGuid = interview.EmployeeGuid,
                                Name = interview.Name,
                                EmailIdle = empIdle.Email,
                                Date = interview.Date,
                                EndContract = interview.EndContract,
                                StartContract = interview.StartContract,
                                Interviewer = empInterviewer.FirstName + " " + empInterviewer.LastName,
                                Idle = empIdle.FirstName + " " + empIdle.LastName,
                                OwnerGuid = interview.OwnerGuid,
                                StatusIdle = empIdle.StatusEmployee.ToString(),
                                Type = interview.Type,
                                Location = interview.Location,
                                StatusInterview = interview.StatusIntervew,
                                rate = rat.Rate,
                                feedback = rat.Feedback

                            }).ToList();

        return Ok(new ResponseOKHandler<IEnumerable<InterviewDto>>(interviewDto));
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
            var result = _interviewRepository.Create(toCreate);

            Rating rating = interviewDto;
            rating.Guid = result.Guid;
            _ratingRepository.Create(rating);

            // Memeriksa apakah penciptaan data berhasil atau gagal.
            if (result is null)
            {
                // Mengembalikan respons BadRequest jika gagal membuat data Interview.
                return BadRequest("Failed to create data");
            }

            // Get employee dengan role "admin"
            var adminEmployee = _employeeRepository.GetAdminEmployee();
            var specificEmployee = _employeeRepository.GetByGuid(interviewDto.EmployeeGuid); 


            string emailTemplatePath = "utilities/TemplateEmail/Schedule.html"; // Sesuaikan path tempat template.
            string emailTemplate = System.IO.File.ReadAllText(emailTemplatePath);

            if (adminEmployee != null)
            {
                emailTemplate = emailTemplate
                      .Replace("{adminName}", adminEmployee.FirstName)
                      .Replace("{interviewName}", interviewDto.Name)
                      .Replace("{interviewDate}", interviewDto.Date.ToString())
                      .Replace("{idleName}", specificEmployee.FirstName + " " + specificEmployee.LastName)
                      .Replace("{emailIdle}", specificEmployee.Email);

                string emailAdmin = Regex.Replace(emailTemplate, "<div id=\"idle\"[^>]*>.*?</div>", "", RegexOptions.Singleline);

                _emailHandler.Send("Schdule Interview", emailAdmin, adminEmployee.Email);
            }

            // Mengirim email ke employee dengan GUID tertentu
            if (specificEmployee != null)
            {
                emailTemplate = emailTemplate
                     .Replace("{interviewName}", interviewDto.Name)
                     .Replace("{interviewDate}", interviewDto.Date.ToString())
                     .Replace("{employeeName}", specificEmployee.FirstName + " " + specificEmployee.LastName);

                string emailIdle = Regex.Replace(emailTemplate, "<div id=\"admin\"[^>]*>.*?</div>", "", RegexOptions.Singleline);

                _emailHandler.Send("Schdule Interview", emailIdle, specificEmployee.Email);
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
    // GET api/interview/ByClientGuid/{clientGuid}
    [HttpGet("GetAllByClientGuid/{companyGuid}")]
    public IActionResult GetByCompanyGuid(Guid companyGuid)
    {
        var interviews = _interviewRepository.GetAllByClientGuid(companyGuid);
        var employees = _employeeRepository.GetAll();

        if (!interviews.Any())
        {
            return NotFound(new ResponseErrorHandler
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data Interview with Specific CompanyGuid Not Found"
            });
        }

        var currentDate = DateTime.Now; 


        var getinterviewDto = (from interview in interviews
                               join empInterviewer in employees on interview.OwnerGuid equals empInterviewer.Guid
                               join empIdle in employees on interview.EmployeeGuid equals empIdle.Guid
                               where empIdle.StatusEmployee == StatusEmployee.idle
                               && interview.EndContract == null && interview.StatusIntervew == null

                               select new GetInterviewDto
                               {
                                   EmployeGuid = empIdle.Guid,
                                   InterviewGuid = interview.Guid,
                                   Idle = empIdle.FirstName + " " + empIdle.LastName,
                                   Foto = empIdle.Foto,
                                   Date = interview.Date,
                               }).ToList();

        return Ok(new ResponseOKHandler<IEnumerable<GetInterviewDto>>(getinterviewDto));
    }

    [HttpGet("GetOnsite/{companyGuid}")]
    public IActionResult GetOnsite(Guid companyGuid)
    {

        var interviews = _interviewRepository.GetAllByClientGuid(companyGuid);
        var employees = _employeeRepository.GetAll();


        if (!interviews.Any())
        {
            return NotFound(new ResponseErrorHandler
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data Interview with Specific CompanyGuid Not Found"
            });
        }

        var getOnsiteDto = (from interview in interviews
                            join empInterviewer in employees on interview.OwnerGuid equals empInterviewer.Guid
                            join empIdle in employees on interview.EmployeeGuid equals empIdle.Guid
                            where empIdle.StatusEmployee == StatusEmployee.onsite && interview.StatusIntervew == StatusIntervew.Lolos && interview.StartContract.HasValue
                            select new GetOnsiteDto
                            {
                                EmployeGuid = empIdle.Guid,
                                InterviewGuid = interview.Guid,
                                Idle = empIdle.FirstName + " " + empIdle.LastName,
                                Foto = empIdle.Foto,
                                StartContract = interview.StartContract,
                                EndContract = interview.EndContract,

                            }).ToList();

        return Ok(new ResponseOKHandler<IEnumerable<GetOnsiteDto>>(getOnsiteDto));
    }

    [HttpGet("GetIdleHistory/{companyGuid}")]
    public IActionResult GetIdleHistory(Guid companyGuid)
    {
        var interviews = _interviewRepository.GetAllByClientGuid(companyGuid);
        var employees = _employeeRepository.GetAll();
        var ratings = _ratingRepository.GetAll();

        if (!interviews.Any())
        {
            return NotFound(new ResponseErrorHandler
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data Interview with Specific CompanyGuid Not Found"
            });
        }

        var idleHistoryDto = (from interview in interviews
                              join rat in ratings on interview.Guid equals rat.Guid
                              join emp in employees on interview.EmployeeGuid equals emp.Guid
                              where interview.EndContract.HasValue && (interview.StatusIntervew == StatusIntervew.ContarctTermination || interview.StatusIntervew == StatusIntervew.EndContract)
                              select new GetIdleHistoryDto
                              {
                                  EmployeeGuid = emp.Guid,
                                  InterviewGuid = interview.Guid,
                                  Idle = emp.FirstName + " " + emp.LastName,
                                  Foto = emp.Foto,
                                  StartContract = interview.StartContract,
                                  EndContract = interview.EndContract,
                                  StatusInterview = interview.StatusIntervew,
                                  Remarks = interview.Remarks,
                                  Rate = rat.Rate.ToString()
                              }).ToList();

        return Ok(new ResponseOKHandler<IEnumerable<GetIdleHistoryDto>>(idleHistoryDto));
    }

    // DELETE api/interview/{guid}
    [HttpDelete("{guid}")]
    public IActionResult Delete(Guid guid)
    {
        try
        {
            // Mengambil data interview berdasarkan GUID
            var rating = _ratingRepository.GetByGuid(guid);
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
            _ratingRepository.Delete(rating);
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
