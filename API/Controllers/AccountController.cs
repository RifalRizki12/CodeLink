using API.Contracts;
using API.DTOs.Accounts;
using API.Models;
using API.Repositories;
using API.Utilities.Handler;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IAccountRoleRepository _accountRoleRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IEmailHandler _emailHandler;
        private readonly ITokenHandlers _tokenHandler;

        // Konstruktor controller yang menerima IAccountRepository sebagai parameter.
        public AccountController(IAccountRepository accountRepository, IEmployeeRepository employeeRepository, IEmailHandler emailHandler, IRoleRepository roleRepository, IAccountRoleRepository accountRoleRepository, ITokenHandlers tokenHandler)
        {
            _accountRepository = accountRepository;
            _employeeRepository = employeeRepository;
            _emailHandler = emailHandler;
            _roleRepository = roleRepository;
            _accountRoleRepository = accountRoleRepository;
            _tokenHandler = tokenHandler;
        }

        //Login
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto request)
        {
            try
            {
                // Validasi input data menggunakan ModelState
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ResponseErrorHandler
                    {
                        Code = StatusCodes.Status400BadRequest,
                        Status = HttpStatusCode.BadRequest.ToString(),
                        Message = "Invalid input !"
                    });
                }

                // Cari pengguna (akun) berdasarkan alamat email
                var user = _accountRepository.GetByEmployeeEmail(request.Email);
                var employee = _employeeRepository.GetByEmployeeEmail(request.Email);

                if (user == null || !HashHandler.VerifyPassword(request.Password, user.Password))
                {
                    return BadRequest(new ResponseErrorHandler
                    {
                        Code = StatusCodes.Status400BadRequest,
                        Status = HttpStatusCode.BadRequest.ToString(),
                        Message = "Account or Password is invalid !",
                    });
                }

                var claims = new List<Claim>();
                claims.Add(new Claim("Email", employee.Email));
                claims.Add(new Claim("Fullname", string.Concat(employee.FirstName + " " + employee.LastName)));
                var getRoleName = from ar in _accountRoleRepository.GetAll()
                                  join r in _roleRepository.GetAll() on ar.RoleGuid equals r.Guid
                                  where ar.AccountGuid == user.Guid
                                  select r.Name;

                foreach (var roleName in getRoleName)
                {
                    claims.Add(new Claim(ClaimTypes.Role, roleName));
                }

                var generateToken = _tokenHandler.Generate(claims);

                // Jika validasi berhasil, kirim respons OK dengan pesan login berhasil
                return Ok(new ResponseOKHandler<object>("Login Success", new { Token = generateToken }));
            }
            catch (Exception ex)
            {
                // Tangani pengecualian dan kembalikan respons 500 Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorHandler
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Error during login",
                    Error = ex.Message
                });
            }
        }


        // Metode untuk mengirim OTP melalui email dalam kasus lupa kata sandi
        [HttpPut("forgot-password")]
        [AllowAnonymous]
        public IActionResult ForgotPassword(string email)
        {
            var employees = _employeeRepository.GetAll();
            var accounts = _accountRepository.GetAll();

            // Memeriksa apakah ada data karyawan dan akun
            if (!(employees.Any() && accounts.Any()))
            {
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data Not Found !"
                });
            }

            if (string.IsNullOrEmpty(email))
            {
                return BadRequest(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Invalid input data !"
                });
            }

            // Cari akun berdasarkan email
            var account = _accountRepository.GetByEmployeeEmail(email);

            if (account == null)
            {
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Email not found."
                });
            }

            // Generate OTP dengan tipe data int
            Random random = new Random();
            int otp = random.Next(100000, 999999);

            // Update akun dengan OTP yang baru dan expired time 30 menit dari sekarang
            account.Otp = otp;
            account.ModifiedDate = DateTime.UtcNow;
            account.ExpiredTime = DateTime.UtcNow.AddMinutes(30); // Set expired time 30 menit dari sekarang
            account.IsUsed = false; // Set ulang IsUsed menjadi false karena ini adalah OTP baru
            _accountRepository.Update(account);

            _emailHandler.Send("Forgot Password", $"Your OTP is {account.Otp}", email);

            // Melakukan join antara tabel Employee, Education, dan University
            var accountDetails = from ac in accounts
                                 join emp in employees on ac.Guid equals emp.Guid
                                 select new ForgotPasswordDto
                                 {
                                     Email = emp.Email,
                                     Otp = ac.Otp,
                                     IsUsed = ac.IsUsed,
                                     ExpiredTime = ac.ExpiredTime,
                                 };

            return Ok(new ResponseOKHandler<IEnumerable<ForgotPasswordDto>>(accountDetails));
        }

        // Metode untuk mengganti kata sandi
        [HttpPut("change-password")]
        [AllowAnonymous]
        public IActionResult ChangePassword([FromBody] ChangePasswordDto request)
        {
            var employees = _employeeRepository.GetAll();
            var accounts = _accountRepository.GetAll();

            // Memeriksa apakah ada data karyawan dan akun
            if (!(employees.Any() && accounts.Any()))
            {
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data Not Found"
                });
            }

            // Cari akun berdasarkan email
            var accountEmail = _accountRepository.GetByEmployeeEmail(request.Email);

            if (accountEmail == null)
            {
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Email not found."
                });
            }

            // Memeriksa OTP
            if (accountEmail.Otp != request.Otp)
            {
                return BadRequest(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Invalid OTP."
                });
            }

            // Memeriksa apakah OTP sudah digunakan
            if (accountEmail.IsUsed)
            {
                return Conflict(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status409Conflict,
                    Status = HttpStatusCode.Conflict.ToString(),
                    Message = "OTP has already been used."
                });
            }

            // Memeriksa apakah OTP sudah kadaluwarsa
            if (accountEmail.ExpiredTime != null && accountEmail.ExpiredTime <= DateTime.UtcNow)
            {
                return BadRequest(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "OTP has expired."
                });
            }

            // Memeriksa apakah NewPassword dan ConfirmPassword cocok
            if (request.NewPassword != request.ConfirmPassword)
            {
                return BadRequest(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "NewPassword and ConfirmPassword do not match."
                });
            }

            // Hash password baru menggunakan bcrypt
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);

            // Update password di akun sebenarnya di database
            accountEmail.Password = hashedPassword;
            accountEmail.ModifiedDate = DateTime.UtcNow;
            accountEmail.IsUsed = true; // Set IsUsed menjadi true karena OTP sudah digunakan
            _accountRepository.Update(accountEmail);

            string responseMessage = "Password has been changed successfully.";

            return Ok(new ResponseOKHandler<string>(responseMessage));
        }

        // GET api/account
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                // Memanggil metode GetAll dari _accountRepository.
                var result = _accountRepository.GetAll();

                // Memeriksa apakah hasil query tidak mengandung data.
                if (!result.Any())
                {
                    return NotFound(new ResponseErrorHandler
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = HttpStatusCode.NotFound.ToString(),
                        Message = "Data Not Found"
                    });
                }

                // Mengonversi hasil query ke objek DTO (Data Transfer Object) menggunakan Select.
                var data = result.Select(x => (AccountDto)x);

                // Mengembalikan data yang ditemukan dalam respons OK.
                return Ok(new ResponseOKHandler<IEnumerable<AccountDto>>(data));
            }
            catch (ExceptionHandler ex)
            {
                // Jika terjadi pengecualian saat mengambil data, akan mengembalikan respons kesalahan dengan pesan pengecualian.
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorHandler
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Failed to retrieve data",
                    Error = ex.Message
                });
            }
        }

        // GET api/account/{guid}
        [HttpGet("{guid}")]
        public IActionResult GetByGuid(Guid guid)
        {
            try
            {
                // Memanggil metode GetByGuid dari _accountRepository dengan parameter GUID.
                var result = _accountRepository.GetByGuid(guid);

                // Memeriksa apakah hasil query tidak ditemukan (null).
                if (result is null)
                {
                    return NotFound(new ResponseErrorHandler
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = HttpStatusCode.NotFound.ToString(),
                        Message = "Id Not Found"
                    });
                }

                // Mengembalikan data yang ditemukan dalam respons OK.
                return Ok(new ResponseOKHandler<AccountDto>((AccountDto)result));
            }
            catch (ExceptionHandler ex)
            {
                // Jika terjadi pengecualian saat mengambil data, akan mengembalikan respons kesalahan dengan pesan pengecualian.
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorHandler
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Failed to retrieve data",
                    Error = ex.Message
                });
            }
        }

        // POST api/account
        [HttpPost]
        public IActionResult Create(CreateAccountDto accountDto)
        {
            try
            {
                // Meng-hash kata sandi sebelum menyimpannya ke database.
                string hashedPassword = HashHandler.HashPassword(accountDto.Password);

                // Mengganti kata sandi asli dengan yang di-hash sebelum menyimpannya ke DTO.
                accountDto.Password = hashedPassword;

                // Memanggil metode Create dari _accountRepository dengan parameter DTO yang sudah di-hash.
                var result = _accountRepository.Create(accountDto);

                // Memeriksa apakah penciptaan data berhasil atau gagal.
                if (result is null)
                {
                    return BadRequest(new ResponseErrorHandler
                    {
                        Code = StatusCodes.Status400BadRequest,
                        Status = HttpStatusCode.BadRequest.ToString(),
                        Message = "Failed to create data"
                    });
                }

                // Mengembalikan data yang berhasil dibuat dalam respons OK.
                return Ok(new ResponseOKHandler<AccountDto>((AccountDto)result));
            }
            catch (ExceptionHandler ex)
            {
                // Jika terjadi pengecualian saat membuat data, akan mengembalikan respons kesalahan dengan pesan pengecualian.
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorHandler
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Failed to create data",
                    Error = ex.Message
                });
            }
        }

        // PUT api/account
        [HttpPut]
        public IActionResult Update(AccountDto accountDto)
        {
            try
            {
                // Memeriksa apakah entitas Account yang akan diperbarui ada dalam database.
                var entity = _accountRepository.GetByGuid(accountDto.Guid);
                if (entity is null)
                {
                    return NotFound(new ResponseErrorHandler
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = HttpStatusCode.NotFound.ToString(),
                        Message = "Id Not Found"
                    });
                }

                // Memeriksa apakah kata sandi berubah.
                if (!string.IsNullOrEmpty(accountDto.Password))
                {
                    // Meng-hash kata sandi baru sebelum menyimpannya ke database.
                    string hashedPassword = HashHandler.HashPassword(accountDto.Password);

                    // Menyalin nilai CreatedDate dari entitas yang ada ke entitas yang akan diperbarui.
                    Account toUpdate = accountDto;
                    toUpdate.CreatedDate = entity.CreatedDate;

                    // Mengganti kata sandi asli dengan yang di-hash pada objek entity.
                    entity.Password = hashedPassword;
                }

                // Memanggil metode Update dari _accountRepository.
                var result = _accountRepository.Update(entity);

                // Memeriksa apakah pembaruan data berhasil atau gagal.
                if (!result)
                {
                    return BadRequest(new ResponseErrorHandler
                    {
                        Code = StatusCodes.Status400BadRequest,
                        Status = HttpStatusCode.BadRequest.ToString(),
                        Message = "Failed to update data"
                    });
                }

                // Mengembalikan pesan sukses dalam respons OK.
                return Ok(new ResponseOKHandler<string>("Data Updated"));
            }
            catch (ExceptionHandler ex)
            {
                // Jika terjadi pengecualian saat mengupdate data, akan mengembalikan respons kesalahan dengan pesan pengecualian.
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorHandler
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Failed to update data",
                    Error = ex.Message
                });
            }
        }

        // DELETE api/account/{guid}
        [HttpDelete("{guid}")]
        public IActionResult Delete(Guid guid)
        {
            try
            {
                // Memanggil metode GetByGuid dari _accountRepository untuk mendapatkan entitas yang akan dihapus.
                var existingAccount = _accountRepository.GetByGuid(guid);

                // Memeriksa apakah entitas yang akan dihapus ada dalam database.
                if (existingAccount is null)
                {
                    return NotFound(new ResponseErrorHandler
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = HttpStatusCode.NotFound.ToString(),
                        Message = "Account not found"
                    });
                }

                // Memanggil metode Delete dari _accountRepository.
                var deleted = _accountRepository.Delete(existingAccount);

                // Memeriksa apakah penghapusan data berhasil atau gagal.
                if (!deleted)
                {
                    return BadRequest(new ResponseErrorHandler
                    {
                        Code = StatusCodes.Status400BadRequest,
                        Status = HttpStatusCode.BadRequest.ToString(),
                        Message = "Failed to delete account"
                    });
                }

                // Mengembalikan kode status 204 (No Content) untuk sukses penghapusan tanpa respons.
                return NoContent();
            }
            catch (ExceptionHandler ex)
            {
                // Jika terjadi pengecualian saat menghapus data, akan mengembalikan respons kesalahan dengan pesan pengecualian.
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorHandler
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = "Failed to delete account",
                    Error = ex.Message
                });
            }
        }
    }
}
