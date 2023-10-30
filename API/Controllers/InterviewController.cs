﻿using API.Contracts;
using API.DTOs.Employees;
using API.DTOs.Interviews;
using API.Models;
using API.Utilities.Enums;
using API.Utilities.Handler;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Net;

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

            // Mengirim email ke employee dengan GUID tertentu
            var specificEmployee = _employeeRepository.GetByGuid(schedule.EmployeeGuid);
            if (specificEmployee != null)
            {

                _emailHandler.Send("Interview Schedule Details",
                    $"Mohon perhatian, akan dilaksanakan {toUpdate.Name} pada\n" +
                    $"Tanggal               : {toUpdate.Date}\n" +
                    $"Pelaksanaan           : {schedule.Type}\n" +
                    $"Lokasi                : {schedule.Location}\n" +
                    $"Nama Company          : {company.Name}\n" +
                    $"Keterangan Tambahan   : {schedule.Remarks}", specificEmployee.Email);
            }


            var employeeOwner = _employeeRepository.GetByGuid(schedule.OwnerGuid);
            if (employeeOwner != null)
            {

                _emailHandler.Send("Interview Schedule Details",
                    $"Mohon perhatian, akan dilaksanakan {toUpdate.Name} pada\n" +
                    $"Tanggal               : {toUpdate.Date}\n" +
                    $"Pelaksanaan           : {schedule.Type}" +
                    $"Lokasi                : {schedule.Location}\n" +
                    $"Nama Company          : {company.Name}\n" +
                    $"Peserta               : {specificEmployee.FirstName} {specificEmployee.LastName}" +
                    $"Keterangan Tambahan   : {schedule.Remarks}", employeeOwner.Email);
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


            if (entity is null)
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

            var result = _interviewRepository.Update(toUpdate);

            Rating rating = announcment;
            rating.Guid = toUpdate.Guid;
            rating.Feedback = announcment.FeedBack;
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

            if (specificEmployee != null)
            {


                if (entity.EndContract.Value.Date == dateNow.Date)
                {
                    _emailHandler.Send("Contract Finished ",
                       $"Kami menguncapkan terimakasi pada saudara/i yang telah bergabung bersama kami. " +
                       $"Semoga dilain waktu kita bisa jumpa kembali " +
                       $"Salam Hormat {company.Name}", specificEmployee.Email);

                    _emailHandler.Send("Contract Finished ",
                       $"Kami menguncapkan terimakasi pada saudara/i yang telah bergabung bersama kami. " +
                       $"Semoga dilain waktu kita bisa jumpa kembali " +
                       $"Salam Hormat {company.Name}", adminEmployee.Email);

                    specificEmployee.StatusEmployee = 0;
                    _employeeRepository.Update(specificEmployee);

                }
                else if (announcment.EndContract != null && announcment.EndContract.Value.Date == dateNow.Date)
                {
                    _emailHandler.Send("Contract Termination ", $"Kami menguncapkan mohon maaf atas ketidaknyamanannya" +
                        $"kami terpaksa harus memutuskan kontrak untuk saudara/i {specificEmployee.FirstName} {specificEmployee.LastName} " +
                        $"dikarenakan  {announcment.Remarks} Terimakasi atas kerjasamanya", specificEmployee.Email);

                    _emailHandler.Send("Contract Termination ", $"Kami menguncapkan mohon maaf atas ketidaknyamanannya" +
                      $"kami terpaksa harus memutuskan kontrak untuk saudara/i {specificEmployee.FirstName} {specificEmployee.LastName} " +
                      $"dikarenakan  {announcment.Remarks} Terimakasi atas kerjasamanya", adminEmployee.Email);


                    specificEmployee.StatusEmployee = 0;
                    _employeeRepository.Update(specificEmployee);
                }
                else
                {
                    string emailBody = $"Terimakasih atas partisipasinya telah mengikuti proses {toUpdate.Name} pada\n" +
                                      $"Date                  : {toUpdate.Date}\n" +
                                      $"kami nyatakan anda    : {announcment.StatusIntervew} \n";

                    if (announcment.StartContract != null && announcment.EndContract != null)
                    {
                        emailBody += $"Start Contract        : {announcment.StartContract} \n" +
                                     $"End Contract          : {announcment.EndContract}\n";

                        specificEmployee.StatusEmployee = (StatusEmployee)2;
                        _employeeRepository.Update(specificEmployee);

                    }
                    emailBody += $"Company Name          : {company.Name}\n" +
                                 $"{announcment.FeedBack}";

                    _emailHandler.Send("Announcment Interview", emailBody, specificEmployee.Email);
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
        // Gabungkan data dari tabel sesuai dengan hubungannya
        var interviewDto = (from interview in interviews
                            join empInterviewer in employees on interview.OwnerGuid equals empInterviewer.Guid
                            join empIdle in employees on interview.EmployeeGuid equals empIdle.Guid
                            select new InterviewDto
                            {
                                Guid = interview.Guid,
                                EmployeeGuid = interview.EmployeeGuid,
                                Name = interview.Name,
                                Date = interview.Date,
                                Interviewer = empInterviewer.FirstName + " " + empInterviewer.LastName,
                                Idle = empIdle.FirstName + " " + empIdle.LastName,
                                OwnerGuid = interview.OwnerGuid,
                            }).ToList();

        return Ok(new ResponseOKHandler<IEnumerable<InterviewDto>>(interviewDto));
/*        // Mengonversi hasil query ke objek DTO (Data Transfer Object) menggunakan Select.
        var data = interviews.Select(x => (InterviewDto)x);

        // Mengembalikan data yang ditemukan dalam respons OK.
        return Ok(new ResponseOKHandler<IEnumerable<InterviewDto>>(data));*/
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
            var specificEmployee = _employeeRepository.GetByGuid(interviewDto.EmployeeGuid); // Ganti dengan metode yang sesuai


            if (adminEmployee != null)
            {
                _emailHandler.Send("Interview Schedule", $"Kami akan mengadakan {interviewDto.Name} pada {interviewDto.Date} untuk Idle dengan nama " +
                    $"{specificEmployee.FirstName + " " + specificEmployee.LastName} Email : {specificEmployee.Email}", adminEmployee.Email);
            }

            // Mengirim email ke employee dengan GUID tertentu
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
