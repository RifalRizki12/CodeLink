using API.DTOs.Employees;
using CLIENT.Contract;
using Microsoft.AspNetCore.Mvc;

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
            var listEmployee = new List<EmployeeDto>();
            if (result != null)
            {

                listEmployee = result.Data.Select(x => (EmployeeDto)x).ToList();
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

    }
}
