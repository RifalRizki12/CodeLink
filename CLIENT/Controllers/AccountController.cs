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

                    // Periksa peran pengguna
                    var user = HttpContext.User;
                    var roleClaim = user.Claims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role");

                    if (roleClaim != null)
                    {
                        string role = roleClaim.Value;
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
                    }
                    else
                    {
                        // Jika login gagal, kirim respons yang berisi pesan kesalahan
                        return Json(new { status = "BadRequest", message = result.Message });
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
