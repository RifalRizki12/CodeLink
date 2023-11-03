using API.DTOs.Accounts;
using API.DTOs.Employees;
using API.DTOs.Roles;
using API.Models;
using CLIENT.Contract;
using CLIENT.Models;
using CLIENT.Repository;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;
using System.Diagnostics;
using System.Net;

namespace CLIENT.Controllers
{
    public class AccountController : Controller
    {

        private readonly IAccountRepository _accountRepository;

        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public IActionResult Logins()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logins([FromBody] LoginDto login)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountRepository.Login(login);

                if (result.Status == "OK")
                {
                    HttpContext.Session.SetString("JWToken", result.Data.Token);

                    // Mengambil klaim pengguna dari JWT
                    var claims = await _accountRepository.GetClaimsAsync(result.Data.Token);

                    if (claims != null)
                    {
                        // Simpan klaim dalam session
                        HttpContext.Session.SetString("FullName", claims.Data.FullName);
                        HttpContext.Session.SetString("EmployeeGuid", claims.Data.EmployeeGuid.ToString());
                        HttpContext.Session.SetString("Email", claims.Data.Email);
                        HttpContext.Session.SetString("Foto", claims.Data.Foto ?? "");
                        // Anda juga bisa menyimpan peran (role) sesuai kebutuhan aplikasi Anda.
                        HttpContext.Session.SetString("Role", claims.Data.Role.FirstOrDefault());

                        string role = HttpContext.Session.GetString("Role"); // Ambil peran dari session

                        // Lakukan pengalihan berdasarkan peran
                        if (role == "admin")
                        {
                            // Pengguna memiliki peran "admin", lakukan tindakan admin
                            return Json(new { redirectTo = Url.Action("Index", "Employee") });
                        }
                        else if (role == "client" || role == "idle")
                        {
                            // Pengguna memiliki peran "client", lakukan tindakan client
                            return Json(new { redirectTo = Url.Action("HomeClient", "HomeClient") });
                        }
                        // Anda juga bisa menambahkan peran lain sesuai dengan kebutuhan Anda
                    }
                    else
                    {
                        // Jika klaim pengguna tidak tersedia
                        return Json(new { status = "BadRequest", message = "User claims not available." });
                    }
                }
            }

            // Jika login gagal atau data yang dikirimkan tidak valid
            return Json(new { redirectTo = Url.Action("Logins", "Account") });
        }


        [HttpGet("Logout/")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Logins", "Account");
        }


        [HttpGet("Account/GuidClient/{guid}")]
        public async Task<JsonResult> GuidClient(Guid guid)
        {
            var result = await _accountRepository.Get(guid);
            var employee = new AccountDto();

            if (result.Data?.Guid != null)
            {
                return Json(result.Data);
            }
            else
            {
                return Json(employee);
            }
        }



        [HttpPut("Account/UpdateClient/{guid}")]
        public async Task<JsonResult> UpdateClient(Guid guid, [FromBody] AccountDto accountDto)
        {
            var response = await _accountRepository.Put(guid, accountDto);

            if (response != null)
            {
                if (response.Code == 200)
                {
                    return Json(new { data = response.Data });
                }
                else
                {
                    return Json(new { error = response.Message });
                }
            }
            else
            {
                return Json(new { error = "An error occurred while updating the employee." });
            }
        }



        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }


        [HttpPut("Account/ForgotPassword/{email}")]
        public async Task<IActionResult> ForgotPassword(string email, [FromBody] ForgotPasswordDto forgotDto)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountRepository.ForgotPassword(email, forgotDto);

                if (result.Status == "OK")
                {
                    //return Json(new { redirectTo = Url.Action("ChangePassword", "Account") });
                    return Json(new { redirectTo = Url.Action("PasswordChange", "Account") });
                }
            }

            // Jika login gagal atau data yang dikirimkan tidak valid
            return Json(new { redirectTo = Url.Action("ChangePassword", "Account") });
        }

        [HttpGet]
        public IActionResult PasswordChange()
        {
            return View();
        }

        [HttpPut("Account/PasswordChange/{email}")]
        public async Task<IActionResult> PasswordChange(string email, [FromBody] ChangePasswordDto changePsswdDto)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountRepository.ChangePassword(email, changePsswdDto);

                if (result.Status == "OK")
                {
                    return Json(new { success = true, redirectTo = Url.Action("Logins", "Account") });

                }
            }

            // Jika login gagal atau data yang dikirimkan tidak valid
            return Json(new { redirectTo = Url.Action("Logins", "Account") });
        }

    }
}
