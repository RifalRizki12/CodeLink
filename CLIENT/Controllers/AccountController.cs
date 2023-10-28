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

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDto login)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountRepository.Login(login);

                if (result.Status == "OK")
                {
                    HttpContext.Session.SetString("JWToken", result.Data.Token);
                    return Json(new { redirectTo = Url.Action("Index", "Employee") });
                }
            }

            // Jika login gagal atau data yang dikirimkan tidak valid
            return Json(new { redirectTo = Url.Action("Login", "Account") });
        }



        [HttpGet("Logout/")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Auth");
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

    }
}
