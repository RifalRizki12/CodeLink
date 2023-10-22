using API.Contracts;
using API.DTOs.Accounts;
using API.DTOs.Employees;
using API.Models;
using API.Repositories;
using API.Utilities.Handler;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Transactions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IAccountRoleRepository _accountRoleRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IExperienceSkillRepository _experienceSkillRepository;
        private readonly IExperienceRepository _experienceRepository;
        private readonly ISkillRepository _skillRepository;
        private readonly IRatingRepository _ratingRepository;

        public EmployeeController(IEmployeeRepository employeeRepository, IAccountRepository accountRepository, IAccountRoleRepository accountRoleRepository, IRoleRepository roleRepository, ICompanyRepository companyRepository, IExperienceSkillRepository experienceSkillRepository, IExperienceRepository experienceRepository, ISkillRepository skillRepository, IRatingRepository ratingRepository)
        {
            _employeeRepository = employeeRepository;
            _accountRepository = accountRepository;
            _accountRoleRepository = accountRoleRepository;
            _roleRepository = roleRepository;
            _companyRepository = companyRepository;
            _experienceSkillRepository = experienceSkillRepository;
            _experienceRepository = experienceRepository;
            _skillRepository = skillRepository;
            _ratingRepository = ratingRepository;
        }


        [HttpGet("GetChart")]
        public IActionResult GetChart()
        {
            var hireEmp = _employeeRepository.GetCaountHired();
            var idleEmp = _employeeRepository.GetCountIdle();
            var company = _companyRepository.GetCaount();

            if ((hireEmp == null) && (idleEmp == null)  && (company == null))
            {
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Employee with Specific GUID Not Found"
                });
            }
            var chartData = new
            {
                HiredEmployeesCount = hireEmp,
                IdleEmployeesCount = idleEmp,
                CompaniesCount = company
            };

            return Ok(new ResponseOKHandler<object>(chartData));

        }



        [HttpPost("registerClient")]
        public IActionResult RegisterClient([FromBody] RegisterClientDto request)
        {
            // Validasi apakah kata sandi dan konfirmasi kata sandi cocok
            if (request.Password != request.ConfirmPassword)
            {
                return BadRequest(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Password and ConfirmPassword do not match."
                });
            }

            using (var transactionScope = new TransactionScope())
            {
                try
                {
                    
                    // Buat entitas Employee yang merupakan pemilik
                    Employee ownerEmployee = request; // Metode ekspresif yang mengkonversi DTO ke Employee

                    // Simpan Employee dalam repository
                    var resultEmp = _employeeRepository.Create(ownerEmployee);

                    // Buat entitas Company dan hubungkan dengan Employee pemilik
                    Company company = request; // Metode ekspresif yang mengkonversi DTO ke Company
                    company.EmployeeGuid = resultEmp.Guid; // Hubungkan Company dengan Employee pemilik

                    // Simpan Company dalam repository
                    var resultEdu = _companyRepository.Create(company);

                    // Buat objek Account dari RegisterDto
                    Account account = request; // Metode ekspresif yang mengkonversi DTO ke Account
                    account.Password = HashHandler.HashPassword(request.Password);

                    // Hubungkan Account dengan Employee pemilik
                    account.Guid = resultEmp.Guid;

                    // Simpan Account dalam repository
                    var resultAcc = _accountRepository.Create(account);

                    //Generate add role user
                    var accountRole = _accountRoleRepository.Create(new AccountRole
                    {
                        AccountGuid = resultEmp.Guid,
                        RoleGuid = _roleRepository.GetDefaultGuid() ?? throw new Exception("Default role not found")
                    });

                    // Commit transaksi jika semua operasi berhasil
                    transactionScope.Complete();

                    return Ok(new ResponseOKHandler<string>("Registration successful, Waiting for Admin Approval"));
                }
                catch (Exception ex)
                {
                    // Rollback transaksi jika terjadi kesalahan
                    transactionScope.Dispose();

                    return BadRequest(new ResponseErrorHandler
                    {
                        Code = StatusCodes.Status400BadRequest,
                        Status = HttpStatusCode.BadRequest.ToString(),
                        Message = "Registration failed. " + ex.Message
                    });
                }
            }
        }

        [HttpGet("allClient-details")]
        public IActionResult GetAllClientDetails()
        {
            try
            {

                // pengambilan data dari tabel Employee
                var employees = _employeeRepository.GetAll();

                // pengambilan data dari tabel Company
                var companies = _companyRepository.GetAll();

                // pengambilan data dari tabel Account
                var accounts = _accountRepository.GetAll();

                // Gabungkan data dari tabel sesuai dengan hubungannya
                var clientDetails = (from emp in employees
                                     join com in companies on emp.Guid equals com.EmployeeGuid
                                     join acc in accounts on emp.Guid equals acc.Guid
                                     select new ClientDetailDto
                                     {
                                         FullName = $"{emp.FirstName} {emp.LastName}",
                                         Gender = emp.Gender.ToString(),
                                         Email = emp.Email,
                                         PhoneNumber = emp.PhoneNumber,
                                         StatusEmployee = emp.Status,
                                         NameCompany = com.Name,
                                         Address = com.Address
                                     }).ToList();

                return Ok(new ResponseOKHandler<IEnumerable<ClientDetailDto>>(clientDetails));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Failed to retrieve client details. " + ex.Message
                });
            }
        }

        [HttpPost("registerIdle")]
        public IActionResult RegisterIdle([FromBody] RegisterIdleDto request)
        {
            // Validasi apakah kata sandi dan konfirmasi kata sandi cocok
            if (request.Password != request.ConfirmPassword)
            {
                return BadRequest(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Password and ConfirmPassword do not match."
                });
            }

            using (var transactionScope = new TransactionScope())
            {
                try
                {
                    // Konversi RegisterIdleDto ke entitas Employee, Account, Experience, dan Skills menggunakan operator konversi implisit
                    Employee employee = request;
                    Account account = request;
                    Experience experience = request;
                    List<Skill> skills = request;

                    // Simpan Employee dalam repository
                    var resultEmp = _employeeRepository.Create(employee);

                    // Simpan Account dalam repository
                    account.Guid = resultEmp.Guid;
                    account.Password = HashHandler.HashPassword(request.Password);
                    var resultAcc = _accountRepository.Create(account);

                    // Simpan Experience dalam repository
                    var resultExp = _experienceRepository.Create(experience);

                    // Simpan Skills dalam repository
                    foreach (var skill in skills)
                    {
                        var resultSkl = _skillRepository.Create(skill);

                        //Generate add Experience_Skill
                        var experienceSkill = _experienceSkillRepository.Create(new ExperienceSkill
                        {
                            EmployeeGuid = resultEmp.Guid,
                            ExperienceGuid = resultExp?.Guid,
                            SkillGuid = resultSkl?.Guid,
                        });
                    }

                    //Generate add role user
                    var accountRole = _accountRoleRepository.Create(new AccountRole
                    {
                        AccountGuid = resultEmp.Guid,
                        RoleGuid = _roleRepository.GetDefaultClient() ?? throw new Exception("Default role not found")
                    });

                    // Commit transaksi jika semua operasi berhasil
                    transactionScope.Complete();

                    return Ok(new ResponseOKHandler<string>("Registration successful !"));
                }
                catch (Exception ex)
                {
                    // Rollback transaksi jika terjadi kesalahan
                    transactionScope.Dispose();

                    return BadRequest(new ResponseErrorHandler
                    {
                        Code = StatusCodes.Status400BadRequest,
                        Status = HttpStatusCode.BadRequest.ToString(),
                        Message = "Registration failed. " + ex.Message
                    });
                }
            }
        }

        // GET api/employee
        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _employeeRepository.GetAll();

            // Periksa jika ada karyawan dalam hasil
            if (!result.Any())
            {
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data Employee Not Found"
                });
            }

            // Konversi hasil ke EmployeeDto dan kembalikan dengan respons 200 OK
            var data = result.Select(x => (EmployeeDto)x);
            return Ok(new ResponseOKHandler<IEnumerable<EmployeeDto>>(data));
        }

        // GET api/employee/{guid}
        [HttpGet("{guid}")]
        public IActionResult GetByGuid(Guid guid)
        {
            var result = _employeeRepository.GetByGuid(guid);

            // Periksa jika karyawan dengan GUID tertentu ada
            if (result is null)
            {
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Employee with Specific GUID Not Found"
                });
            }

            // Konversi hasil ke EmployeeDto dan kembalikan dengan respons 200 OK
            return Ok(new ResponseOKHandler<EmployeeDto>((EmployeeDto)result));
        }

        // POST api/employee
        [HttpPost]
        public IActionResult Create(CreateEmployeeDto employeeDto)
        {
            try
            {

                // Buat instance Employee baru dari DTO yang diberikan
                Employee toCreate = employeeDto;

                // Panggil repositori untuk membuat karyawan
                var result = _employeeRepository.Create(toCreate);

                // Kembalikan karyawan yang telah dibuat dengan respons 200 OK
                return Ok(new ResponseOKHandler<EmployeeDto>((EmployeeDto)result));
            }
            catch (Exception ex)
            {
                // Tangani pengecualian dan kembalikan respons 500 Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorHandler
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Failed to create employee data",
                    Error = ex.Message
                });
            }
        }

        // PUT api/employee
        [HttpPut]
        public IActionResult Update(EmployeeDto employeeDto)
        {
            try
            {
                // Dapatkan entitas karyawan yang akan diperbarui berdasarkan GUID
                var entity = _employeeRepository.GetByGuid(employeeDto.Guid);

                // Periksa jika entitas ada
                if (entity is null)
                {
                    return NotFound(new ResponseErrorHandler
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = HttpStatusCode.NotFound.ToString(),
                        Message = "Employee with Specific GUID Not Found"
                    });
                }

                // Ubah beberapa properti karyawan dan panggil repositori untuk memperbarui
                Employee toUpdate = employeeDto;
                toUpdate.ModifiedDate = entity.ModifiedDate;

                var result = _employeeRepository.Update(toUpdate);

                // Kembalikan pesan sukses dengan respons 200 OK
                // Menggunakan ResponseOKHandler untuk memberikan respons sukses
                var response = new ResponseOKHandler<EmployeeDto>("Employee Data Has Been Updated");

                return Ok(response);
            }
            catch (Exception ex)
            {
                // Tangani pengecualian dan kembalikan respons 500 Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorHandler
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Gagal memperbarui data karyawan",
                    Error = ex.Message
                });
            }
        }

        [HttpGet("detailsIdle")]
        public IActionResult GetDetails()
        {
            var employees = _employeeRepository.GetAll();
            var companies = _companyRepository.GetAll();
            var skills = _skillRepository.GetAll();
            var experiences = _experienceRepository.GetAll();
            var experienceSkills = _experienceSkillRepository.GetAll();
            var ratings = _ratingRepository.GetAll();

            if (!(employees.Any() && companies.Any() && skills.Any() && experiences.Any() && experienceSkills.Any()))
            {
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data Employee Tidak Ditemukan"
                });
            }

            var avgRatings = from rate in ratings
                             group rate by rate.EmployeeGuid into ratingGroup
                             select new
                             {
                                 EmployeeGuid = ratingGroup.Key,
                                 AvgRating = ratingGroup.Average(r => r.Rate)
                             };


            var employeeDetails = from emp in employees
                                  join expSkill in experienceSkills on emp.Guid equals expSkill.EmployeeGuid into expSkillJoined
                                  from expSkillResult in expSkillJoined.DefaultIfEmpty()
                                  join exp in experiences on expSkillResult?.ExperienceGuid equals exp.Guid into expJoined
                                  from expResult in expJoined.DefaultIfEmpty()
                                  join skill in skills on expSkillResult?.SkillGuid equals skill.Guid into skillJoined
                                  from skillResult in skillJoined.DefaultIfEmpty()
                                  join com in companies on emp.CompanyGuid equals com.Guid into companyJoined
                                  from company in companyJoined.DefaultIfEmpty()
                                  join owner in employees on company?.EmployeeGuid equals owner.Guid into ownerJoined
                                  from companyOwner in ownerJoined.DefaultIfEmpty()
                                  join avgRate in avgRatings on emp.Guid equals avgRate.EmployeeGuid into avgRatingJoined
                                  from avgRatingResult in avgRatingJoined.DefaultIfEmpty()
                                  where emp.Status == "pekerja"|| emp.Status=="idle"
                                  select new EmployeeDetailDto
                                  {
                                      FullName = emp.FirstName + " " + emp.LastName,
                                      Gender = emp.Gender.ToString(),
                                      Email = emp.Email,
                                      PhoneNumber = emp.PhoneNumber,
                                      StatusEmployee = emp.Status,
                                      AverageRating = avgRatingResult?.AvgRating ?? 0,
                                      HardSkill = skillResult?.Hard ?? "N/A",
                                      SoftSkill = skillResult?.Soft ?? "N/A",
                                      NameCompany = company?.Name ?? "N/A",
                                      Address = company?.Address ?? "N/A",
                                      OwnerGuid = company?.EmployeeGuid ?? Guid.Empty,
                                      EmployeeOwner = companyOwner?.FirstName + " " + companyOwner?.LastName ?? "N/A",
                                      Experience = expResult?.Name ?? "N/A",
                                      Position = expResult?.Position ?? "N/A",
                                      CompanyExperience = expResult?.Company ?? "N/A"
                                  };

            return Ok(new ResponseOKHandler<IEnumerable<EmployeeDetailDto>>(employeeDetails));
        }





        // DELETE api/employee/{guid}
        [HttpDelete("{guid}")]
        public IActionResult Delete(Guid guid)
        {
            try
            {
                // Dapatkan entitas karyawan yang akan dihapus berdasarkan GUID
                var employeeGuid = _employeeRepository.GetByGuid(guid);
                var accountGuid = _accountRepository.GetByGuid(guid);

                // Periksa jika entitas ada
                if (employeeGuid is null || accountGuid is null)
                {
                    return NotFound(new ResponseErrorHandler
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = HttpStatusCode.NotFound.ToString(),
                        Message = "Specific GUID Not Found"
                    });
                }

                // Periksa apakah ada referensi ke karyawan di entitas lain
                bool isReferenced = accountGuid != null && accountGuid.Guid == employeeGuid.Guid;

                if (isReferenced)
                {
                    return BadRequest(new ResponseErrorHandler
                    {
                        Code = StatusCodes.Status400BadRequest,
                        Status = HttpStatusCode.BadRequest.ToString(),
                        Message = "Cannot delete employee because it is still used by another entity!",
                    });
                }

                // Hapus karyawan dari repositori
                _employeeRepository.Delete(employeeGuid);

                // Kembalikan pesan sukses dengan respons 200 OK
                return Ok(new ResponseOKHandler<string>("Employee Data Has Been Deleted"));
            }
            catch (ExceptionHandler ex)
            {
                // Tangani pengecualian dan kembalikan respons 500 Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorHandler
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Failed to delete employee data",
                    Error = ex.Message
                });
            }
        }

    }
}
