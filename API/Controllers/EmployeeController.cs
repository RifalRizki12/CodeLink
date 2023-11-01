﻿using API.Contracts;
using API.DTOs.Accounts;
using API.DTOs.Employees;
using API.Models;
using API.Repositories;
using API.Utilities.Enums;
using API.Utilities.Handler;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        private readonly IRoleRepository _roleRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly ICurriculumVitaeRepository _curriculumVitaeRepository;
        private readonly ISkillRepository _skillRepository;
        private readonly IRatingRepository _ratingRepository;
        private readonly IInterviewRepository _interviewRepository;


        public EmployeeController(IEmployeeRepository employeeRepository, IAccountRepository accountRepository, IRoleRepository roleRepository, ICompanyRepository companyRepository, ICurriculumVitaeRepository curriculumVitaeRepository, ISkillRepository skillRepository, IRatingRepository ratingRepository, IInterviewRepository interviewRepository)
        {
            _employeeRepository = employeeRepository;
            _accountRepository = accountRepository;
            _roleRepository = roleRepository;
            _companyRepository = companyRepository;
            _curriculumVitaeRepository = curriculumVitaeRepository;
            _skillRepository = skillRepository;
            _ratingRepository = ratingRepository;
            _interviewRepository = interviewRepository;
        }

        [HttpGet("GetChart")]
        public IActionResult GetChart()
        {
            var hireEmp = _employeeRepository.GetCaountHired();
            var idleEmp = _employeeRepository.GetCountIdle();
            var company = _companyRepository.GetCaount();

            if (hireEmp == null && idleEmp == null && company == null)
            {
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data not found"
                });
            }

            var chartData = new ChartDto
            {
                HiredEmployeesCount = hireEmp,
                IdleEmployeesCount = idleEmp,
                CompaniesCount = company
            };

            return Ok(new ResponseOKHandler<ChartDto>(chartData));
        }

        [HttpPost("registerClient")]
        public async Task<IActionResult> RegisterClient([FromForm]RegisterClientDto registrationDto)
        {
            if (ModelState.IsValid)
            {
                using (var transactionScope = new TransactionScope())
                {
                    try
                    {
                        // Konversi DTO menjadi objek Account, Employee, dan Company
                        Account account = registrationDto;
                        Employee employee = registrationDto;
                        Company company = registrationDto;

                        // Handle pengunggahan foto
                        if (registrationDto.ProfilePictureFile != null && registrationDto.ProfilePictureFile.Length > 0)
                        {
                            string originalFileName = Path.GetFileName(registrationDto.ProfilePictureFile.FileName);
                            string uniqueFileName = $"{DateTime.Now:yyyyMMddHHmmssfff}_{Guid.NewGuid()}_{originalFileName}";
                            string uploadPath = "Utilities/File/ProfilePictures/"; // Ganti dengan direktori yang sesuai
                            string filePath = Path.Combine(uploadPath, uniqueFileName);

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                registrationDto.ProfilePictureFile.CopyTo(stream);
                            }

                            // Simpan nama berkas unik ke atribut Foto pada objek Employee
                            employee.Foto = uniqueFileName;
                        }

                        // Hash password menggunakan bcrypt
                        string hashedPassword = HashHandler.HashPassword(registrationDto.Password);

                        // Simpan Employee dalam repository
                        _employeeRepository.Create(employee);

                        // Hubungkan Company dengan Employee pemilik
                        company.EmployeeGuid = employee.Guid;

                        // Simpan Company dalam repository
                        _companyRepository.Create(company);

                        // Hubungkan Account dengan Employee pemilik
                        account.Guid = employee.Guid;
                        account.RoleGuid = _roleRepository.GetDefaultGuid() ?? throw new Exception("Default role not found");
                        account.Password = hashedPassword;

                        // Simpan Account dalam repository
                        _accountRepository.Create(account);

                        // Commit transaksi jika semua operasi berhasil
                        transactionScope.Complete();

                        return Ok(new ResponseOKHandler<string>("Registration successful, Waiting for Admin Approval"));
                    }
                    catch (Exception ex)
                    {
                        // Rollback transaksi jika terjadi kesalahan
                        return BadRequest(new ResponseErrorHandler
                        {
                            Code = StatusCodes.Status400BadRequest,
                            Status = HttpStatusCode.BadRequest.ToString(),
                            Message = "Registration failed. " + ex.Message
                        });
                    }
                }
            }

            return BadRequest(new ResponseErrorHandler
            {
                Code = StatusCodes.Status400BadRequest,
                Status = HttpStatusCode.BadRequest.ToString(),
                Message = "Invalid request data."
            });
        }

        [HttpPut("updateClient")]
        public async Task<IActionResult> UpdateClient([FromForm] UpdateClientDto updateClientDto)
        {
            if (ModelState.IsValid)
            {
                using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    try
                    {
                        // Ambil perusahaan berdasarkan GUID yang diberikan
                        Company existingCompany = _companyRepository.GetByGuid(updateClientDto.CompanyGuid);
                        if (existingCompany == null)
                        {
                            return NotFound(new ResponseErrorHandler
                            {
                                Code = StatusCodes.Status404NotFound,
                                Status = HttpStatusCode.NotFound.ToString(),
                                Message = "Company not found."
                            });
                        }

                        // Update informasi perusahaan
                        existingCompany.Name = updateClientDto.NameCompany;
                        existingCompany.Address = updateClientDto.AddressCompany;
                        existingCompany.Description = updateClientDto.Description;
                        _companyRepository.Update(existingCompany);

                        // Ambil karyawan berdasarkan EmployeeGuid yang diberikan dalam DTO
                        Employee existingEmployee = _employeeRepository.GetByGuid(updateClientDto.EmployeeGuid);
                        if (existingEmployee == null)
                        {
                            return NotFound(new ResponseErrorHandler
                            {
                                Code = StatusCodes.Status404NotFound,
                                Status = HttpStatusCode.NotFound.ToString(),
                                Message = "Employee not found."
                            });
                        }

                        // Update informasi karyawan
                        existingEmployee.FirstName = updateClientDto.FirstName;
                        existingEmployee.LastName = updateClientDto.LastName;
                        existingEmployee.Email = updateClientDto.Email;
                        existingEmployee.Gender = updateClientDto.Gender;
                        existingEmployee.PhoneNumber = updateClientDto.PhoneNumber;

                        // Handle update gambar profil jika ada
                        if (updateClientDto.ProfilePictureFile != null && updateClientDto.ProfilePictureFile.Length > 0)
                        {
                            // Path ke direktori penyimpanan gambar profil
                            string uploadPath = "Utilities/File/ProfilePictures/";

                            // Jika gambar profil lama ada, hapus gambar lama
                            if (!string.IsNullOrEmpty(existingEmployee.Foto))
                            {
                                string oldFilePath = Path.Combine(uploadPath, existingEmployee.Foto);
                                if (System.IO.File.Exists(oldFilePath))
                                {
                                    System.IO.File.Delete(oldFilePath);
                                }
                            }

                            // Generate nama unik untuk file gambar baru
                            string uniqueFileName = $"{DateTime.Now:yyyyMMddHHmmssfff}_{Guid.NewGuid()}{Path.GetFileName(updateClientDto.ProfilePictureFile.FileName)}";
                            string filePath = Path.Combine(uploadPath, uniqueFileName);

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await updateClientDto.ProfilePictureFile.CopyToAsync(stream);
                            }

                            // Update atribut Foto dalam objek existingEmployee
                            existingEmployee.Foto = uniqueFileName;
                        }

                        // Update karyawan dalam repository
                        _employeeRepository.Update(existingEmployee);

                     
                        transactionScope.Complete();
                        return Ok(new ResponseOKHandler<string>("Update successful!"));
                    }
                    catch (Exception ex)
                    {
                        transactionScope.Dispose();
                        return BadRequest(new ResponseErrorHandler
                        {
                            Code = StatusCodes.Status400BadRequest,
                            Status = HttpStatusCode.BadRequest.ToString(),
                            Message = "Update failed. " + ex.Message
                        });
                    }
                }
            }

            return BadRequest(new ResponseErrorHandler
            {
                Code = StatusCodes.Status400BadRequest,
                Status = HttpStatusCode.BadRequest.ToString(),
                Message = "Invalid request data."
            });
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
                var role = _roleRepository.GetAll();

                // Gabungkan data dari tabel sesuai dengan hubungannya
                var clientDetails = (from emp in employees
                                     join com in companies on emp.Guid equals com.EmployeeGuid
                                     join acc in accounts on emp.Guid equals acc.Guid
                                     join r in role on acc.RoleGuid equals r.Guid
                                     select new ClientDetailDto
                                     {
                                         EmployeeGuid = emp.Guid,
                                         CompanyGuid = com.Guid,
                                         FullName = $"{emp.FirstName} {emp.LastName}",
                                         Gender = emp.Gender.ToString(),
                                         Email = emp.Email,
                                         FotoEmployee = emp.Foto,
                                         StatusAccount = acc.Status.ToString(),
                                         PhoneNumber = emp.PhoneNumber,
                                         StatusEmployee = emp.StatusEmployee.ToString(),
                                         NameCompany = com.Name,
                                         Address = com.Address,
                                         RoleName = r.Name
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

        [HttpPost("RegisterIdle")]
        public async Task<IActionResult> RegisterIdle([FromForm]RegisterIdleDto registrationDto)
        {
            if (ModelState.IsValid)
            {
                using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    try
                    {
                        Employee employee = registrationDto;
                        Account account = registrationDto;
                        CurriculumVitae curriculumVitae = registrationDto;
                        List<Skill> skills = registrationDto;


                        // Handle pengunggahan foto (jika diperlukan)
                        if (registrationDto.ProfilePictureFile != null && registrationDto.ProfilePictureFile.Length > 0)
                        {
                            string originalFileName = Path.GetFileName(registrationDto.ProfilePictureFile.FileName);
                            string uniqueFileName = $"{DateTime.Now:yyyyMMddHHmmssfff}_{Guid.NewGuid()}_{originalFileName}";
                            string uploadPath = "Utilities/File/ProfilePictures/"; // Ganti dengan direktori yang sesuai
                            string filePath = Path.Combine(uploadPath, uniqueFileName);

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await registrationDto.ProfilePictureFile.CopyToAsync(stream);
                            }

                            // Simpan nama berkas unik ke atribut Foto pada objek Employee
                            employee.Foto = uniqueFileName;
                        }

                        // Handle pengunggahan CV (jika diperlukan)
                        if (registrationDto.CvFile != null && registrationDto.CvFile.Length > 0)
                        {
                            string originalFileName = Path.GetFileName(registrationDto.CvFile.FileName);
                            string uniqueFileName = $"{DateTime.Now:yyyyMMddHHmmssfff}_{Guid.NewGuid()}_{originalFileName}";
                            string uploadPath = "Utilities/File/Cv/"; // Ganti dengan direktori yang sesuai
                            string filePath = Path.Combine(uploadPath, uniqueFileName);

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await registrationDto.CvFile.CopyToAsync(stream);
                            }

                            // Simpan nama berkas unik ke atribut CV pada objek CurriculumVitae
                            curriculumVitae.Cv = uniqueFileName;
                        }

                        // Simpan Employee dalam repository
                        var resultEmp = _employeeRepository.Create(employee);

                        // Simpan Account dalam repository
                        account.Guid = resultEmp.Guid;
                        account.Password = HashHandler.HashPassword(registrationDto.Password);
                        account.RoleGuid = _roleRepository.GetDefaultClient() ?? throw new Exception("Default role not found");
                        var resultAcc = _accountRepository.Create(account);

                        // Simpan CurriculumVitae dalam repository
                        curriculumVitae.Guid = resultEmp.Guid;
                        var resultCv = _curriculumVitaeRepository.Create(curriculumVitae);

                        // Simpan Skills dalam repository
                        foreach (var skill in skills)
                        {
                            skill.CvGuid = resultCv.Guid; // Sesuaikan ini dengan hubungan yang benar
                            var resultSkl = _skillRepository.Create(skill);
                        }


                        // Commit transaksi jika semua operasi berhasil
                        transactionScope.Complete();

                        return Ok(new ResponseOKHandler<string>("Registration successful!"));
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

            return BadRequest(new ResponseErrorHandler
            {
                Code = StatusCodes.Status400BadRequest,
                Status = HttpStatusCode.BadRequest.ToString(),
                Message = "Invalid request data."
            });
        }

        [HttpPut("UpdateIdle")]
        public async Task<IActionResult> UpdateIdle([FromForm] UpdateIdleDto updateDto)
        {
            if (ModelState.IsValid)
            {
                using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    try
                    {
                        Employee existingEmployee = _employeeRepository.GetByGuid(updateDto.Guid);
                        if (existingEmployee == null)
                        {
                            return NotFound(new ResponseErrorHandler
                            {
                                Code = StatusCodes.Status404NotFound,
                                Status = HttpStatusCode.NotFound.ToString(),
                                Message = "User not found."
                            });
                        }

                        // Update the existingEmployee object with data from the updateDto
                        existingEmployee.FirstName = updateDto.FirstName;
                        existingEmployee.LastName = updateDto.LastName;
                        existingEmployee.Email = updateDto.Email;
                        existingEmployee.PhoneNumber = updateDto.PhoneNumber;
                        existingEmployee.StatusEmployee = updateDto.StatusEmployee;
/*                        existingEmployee.CompanyGuid = updateDto.CompanyGuid;*/


                        // Update other properties as needed

                        // Handle update gambar profil jika ada
                        if (updateDto.ProfilePictureFile != null && updateDto.ProfilePictureFile.Length > 0)
                        {
                            // Path ke direktori penyimpanan gambar profil
                            string uploadPath = "Utilities/File/ProfilePictures/"; // Update to the appropriate directory

                            // Jika gambar profil lama ada, hapus gambar lama
                            if (!string.IsNullOrEmpty(existingEmployee.Foto))
                            {
                                string oldFilePath = Path.Combine(uploadPath, existingEmployee.Foto);
                                if (System.IO.File.Exists(oldFilePath))
                                {
                                    System.IO.File.Delete(oldFilePath);
                                }
                            }

                            // Generate nama unik untuk file gambar baru
                            string uniqueFileName = $"{DateTime.Now:yyyyMMddHHmmssfff}_{Guid.NewGuid()}{Path.GetFileName(updateDto.ProfilePictureFile.FileName)}";
                            string filePath = Path.Combine(uploadPath, uniqueFileName);

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await updateDto.ProfilePictureFile.CopyToAsync(stream);
                            }

                            // Update atribut Foto dalam objek existingEmployee
                            existingEmployee.Foto = uniqueFileName;
                        }

                        // Update the Employee in the repository
                        _employeeRepository.Update(existingEmployee);

                        // Update the Account in the repository (if needed)
                       


                        // Update CurriculumVitae (if needed)
                        CurriculumVitae existingCv = _curriculumVitaeRepository.GetByGuid(existingEmployee.Guid);

                        // Handle update berkas CV jika ada
                        if (updateDto.CvFile != null && updateDto.CvFile.Length > 0)
                        {
                            // Path ke direktori penyimpanan berkas CV
                            string uploadPath = "Utilities/File/Cv/"; // Ganti dengan direktori yang sesuai

                            // Jika berkas CV lama ada, hapus berkas lama
                            if (!string.IsNullOrEmpty(existingCv.Cv))
                            {
                                string oldFilePath = Path.Combine(uploadPath, existingCv.Cv);
                                if (System.IO.File.Exists(oldFilePath))
                                {
                                    System.IO.File.Delete(oldFilePath);
                                }
                            }

                            // Generate nama unik untuk file CV baru
                            string uniqueFileName = $"{DateTime.Now:yyyyMMddHHmmssfff}_{Guid.NewGuid()}{Path.GetFileName(updateDto.CvFile.FileName)}";
                            string filePath = Path.Combine(uploadPath, uniqueFileName);

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await updateDto.CvFile.CopyToAsync(stream);
                            }

                            // Simpan nama berkas unik ke atribut CV dalam objek CurriculumVitae
                            existingCv.Cv = uniqueFileName;
                        }

                        _curriculumVitaeRepository.Update(existingCv);


                        List<Skill> existingSkills = _skillRepository.GetSkillsByCvGuid(existingCv.Guid);
                        foreach (var skill in existingSkills)
                        {
                            _skillRepository.Delete(skill);
                        }

                        // Kemudian tambahkan skill baru dari updateDto
                        if (updateDto.Skills != null && updateDto.Skills.Any())
                        {
                            foreach (var skillName in updateDto.Skills)
                            {
                                var newSkill = new Skill
                                {
                                    Name = skillName,
                                    CvGuid = existingCv.Guid
                                };
                                _skillRepository.Add(newSkill);
                            }
                        }

                        transactionScope.Complete();

                        return Ok(new ResponseOKHandler<string>("Update successful!"));
                    }
                    catch (Exception ex)
                    {
                        // Rollback the transaction in case of an error
                        transactionScope.Dispose();

                        return BadRequest(new ResponseErrorHandler
                        {
                            Code = StatusCodes.Status400BadRequest,
                            Status = HttpStatusCode.BadRequest.ToString(),
                            Message = "Update failed. " + ex.Message
                        });
                    }
                }
            }

            return BadRequest(new ResponseErrorHandler
            {
                Code = StatusCodes.Status400BadRequest,
                Status = HttpStatusCode.BadRequest.ToString(),
                Message = "Invalid request data."
            });
        }

        [HttpGet("detailsIdle")]
        public IActionResult GetDetails()
        {
            var employees = _employeeRepository.GetAll();
            var companies = _companyRepository.GetAll();
            var skills = _skillRepository.GetAll();
            var curriculumVitae = _curriculumVitaeRepository.GetAll();
            var interviews = _interviewRepository.GetAll();
            var ratings = _ratingRepository.GetAll();

            if (!(employees.Any() || companies.Any() || skills.Any() || curriculumVitae.Any()))
            {
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data Employee Tidak Ditemukan"
                });
            }

            var avgRatings = from emp in employees
                             join interview in interviews on emp.Guid equals interview.EmployeeGuid into interviewGroup

                             select new
                             {
                                 Employee = emp,
                                 AvgRating = interviewGroup.Select(i => i.Rating.Rate).Average()
                             };

            var employeeDetails = from emp in employees
                                  join cuVit in curriculumVitae on emp.Guid equals cuVit.Guid into cuVitJoined
                                  from cuVitResult in cuVitJoined.DefaultIfEmpty()
                                  join com in companies on emp.CompanyGuid equals com.Guid into companyJoined
                                  from company in companyJoined.DefaultIfEmpty()
                                  join owner in employees on company?.EmployeeGuid equals owner.Guid into ownerJoined
                                  from companyOwner in ownerJoined.DefaultIfEmpty()
                                  join avgRating in avgRatings on emp.Guid equals avgRating.Employee.Guid into avgRatingJoined
                                  from avgRatingResult in avgRatingJoined.DefaultIfEmpty()
                                  where emp.StatusEmployee == StatusEmployee.idle
                                  select new EmployeeDetailDto
                                  {
                                      Guid = emp.Guid,
                                      FullName = emp.FirstName + " " + emp.LastName,
                                      Gender = emp.Gender.ToString(),
                                      Email = emp.Email,
                                      PhoneNumber = emp.PhoneNumber,
                                      Grade = emp.Grade.ToString(),
                                      StatusEmployee = emp.StatusEmployee.ToString(),
                                      Foto = emp.Foto, // Memanggil fungsi GetPhotoUrl
                                      AverageRating = avgRatingResult?.AvgRating ?? 0,
                                      Skill = skills
                                              .Where(skill => skill.CvGuid == cuVitResult?.Guid)
                                              .Select(skill => skill.Name)
                                              .ToList(),
                                      Cv = cuVitResult?.Cv,
                                      NameCompany = company?.Name ?? "N/A",
                                      Address = company?.Address ?? "N/A",
                                      OwnerGuid = company?.EmployeeGuid ?? Guid.Empty,
                                      EmployeeOwner = companyOwner?.FirstName + " " + companyOwner?.LastName ?? "N/A",
                                  };

            return Ok(new ResponseOKHandler<IEnumerable<EmployeeDetailDto>>(employeeDetails));
        }

        [HttpGet("getByGuidIdle/{employeeGuid}")]
        public IActionResult GetEmployeeDetails(Guid employeeGuid)
        {
            // Get the employee by GUID
            Employee employee = _employeeRepository.GetByGuid(employeeGuid);

            if (employee == null)
            {
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Employee not found."
                });
            }

            // Get the related data (skills and curriculum vitae)
            List<Skill> skills = _skillRepository.GetSkillsByCvGuid(employee.Guid);
            CurriculumVitae curriculumVitae = _curriculumVitaeRepository.GetByGuid(employee.Guid);

            // Initialize an EmployeeDetailDto with default values
            EmployeeDetailDto employeeDetail = new EmployeeDetailDto
            {
                Guid = employee.Guid,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                FullName = employee.FirstName + " " + employee.LastName,
                Gender = employee.Gender.ToString(),
                Email = employee.Email,
                PhoneNumber = employee.PhoneNumber,
                Grade = employee.Grade.ToString(),
                StatusEmployee = employee.StatusEmployee.ToString(),
                Foto = employee.Foto,
                Skill = null,
                Cv = null,
                NameCompany = null,
                Address = null,
                OwnerGuid = Guid.Empty,
                EmployeeOwner = "N/A",
                AverageRating = null,
                // Add other attributes as needed
            };

            if (skills.Count > 0 )
            {
                employeeDetail.Skill = skills.Select(skill => skill.Name).ToList();
            }

            if (curriculumVitae != null)
            {
                employeeDetail.Cv = curriculumVitae?.Cv;
            }

            // Find the company where the employee works
            Company company = _companyRepository.GetByGuid(employee.CompanyGuid.GetValueOrDefault());
            
            if (company != null)
            {
                // Employee has a company, update company-related fields in EmployeeDetailDto
                employeeDetail.NameCompany = company.Name;
                employeeDetail.Address = company.Address;

                // Find the owner of the company
                Employee companyOwner = _employeeRepository.GetByGuid(company.EmployeeGuid.GetValueOrDefault());

                employeeDetail.OwnerGuid = companyOwner?.Guid ?? Guid.Empty;
                employeeDetail.EmployeeOwner = companyOwner?.FirstName + " " + companyOwner?.LastName ?? "N/A";
            }

            Interview interview = _interviewRepository.GetEmployeeGuid(employee.Guid);

            if (interview != null)
            {
                // Get the rating for the interview
                Rating rating = _ratingRepository.GetByGuid(interview.Guid);
                employeeDetail.AverageRating = rating?.Rate;
            }

            return Ok(new ResponseOKHandler<EmployeeDetailDto>(employeeDetail));
        }

        [HttpGet("getByGuidClient/{companyGuid}")]
        public IActionResult GetAllClientDetails(Guid companyGuid)
        {
            try
            {
                // Get the employee by GUID
                Company company = _companyRepository.GetByGuid(companyGuid);

                if (company == null)
                {
                    return NotFound(new ResponseErrorHandler
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = HttpStatusCode.NotFound.ToString(),
                        Message = "Company not found."
                    });
                }
                Employee employee = _employeeRepository.GetByGuid(company.EmployeeGuid);

                // Get the company and account details
                Account account = _accountRepository.GetByGuid(employee.Guid);
                Role role = _roleRepository.GetByGuid(account.Guid);

                ClientDetailDto clientDetail = new ClientDetailDto
                {
                    EmployeeGuid = employee.Guid,
                    CompanyGuid = company.Guid,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName, 
                    Gender = employee.Gender.ToString(),
                    Email = employee.Email,
                    FotoEmployee = employee.Foto,
                    StatusAccount = account.Status.ToString(),
                    PhoneNumber = employee.PhoneNumber,
                    StatusEmployee = employee.StatusEmployee.ToString(),
                    NameCompany = company?.Name,
                    Address = company?.Address,
                    Description = company?.Description,
                    RoleName = role?.Name,
                    
                };

                return Ok(new ResponseOKHandler<ClientDetailDto>(clientDetail));
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
