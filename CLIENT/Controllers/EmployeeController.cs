using API.DTOs.Accounts;
using API.DTOs.Employees;
using API.Utilities.Handler;
using CLIENT.Contract;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CLIENT.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository repository;

        public EmployeeController(IEmployeeRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IActionResult> List()
        {
            var result = await repository.Get();
            var listEmployee = new List<EmployeeDetailDto>();
            if (result != null)
            {

                listEmployee = result.Data.Select(x => (EmployeeDetailDto)x).ToList();
            }

            return View(listEmployee);
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<JsonResult> GetEmployeeData()
        {
            var result = await repository.GetDetailIdle();
            return Json(new { data = result.Data });
        }

        [HttpPost]
        public async Task<IActionResult> RegisterIdle(RegisterIdleDto registrationDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Kirim data ke metode RegisterIdle di repository
                    var result = await repository.RegisterIdle(registrationDto);

                    if (result != null)
                    {
                        // Kembalikan respons dari repository dalam bentuk JSON
                        return Json(result);
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
