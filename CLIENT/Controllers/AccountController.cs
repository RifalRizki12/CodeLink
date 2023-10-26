using API.DTOs.Accounts;
using API.DTOs.Employees;
using API.Utilities.Handler;
using CLIENT.Contract;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;
using System.Collections.Generic;
using System.Net;

namespace CLIENT.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountRepository repository;

        public AccountController(IAccountRepository repository)
        {
            this.repository = repository;
        }

       
        public async Task<IActionResult> CreateIdle()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateIdle(RegisterIdleDto registrationDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Kirim data ke metode RegisterIdle di repository
                    var result = await repository.Post(registrationDto);
                    if (result.Code == 200)
                    {
                        return RedirectToAction(nameof(CreateIdle));
                    }
                    else if (result.Code == 409)
                    {
                        ModelState.AddModelError(string.Empty, result.Message);
                        return View();
                    }
                }
                catch (Exception ex)
                {
                    // Tangani kesalahan internal
                    return Json(new ResponseErrorHandler
                    {
                        Code = StatusCodes.Status500InternalServerError,
                        Status = HttpStatusCode.InternalServerError.ToString(),
                        Message = "Internal server error: " + ex.Message
                    });
                }
            }

            return BadRequest(new ResponseErrorHandler
            {
                Code = StatusCodes.Status400BadRequest,
                Status = HttpStatusCode.BadRequest.ToString(),
                Message = "Invalid request data." 
            });
        }

    }
}
