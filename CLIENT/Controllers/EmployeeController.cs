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

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> List()
        {
            var result = await repository.Get(); // Menggunakan metode Get dari repository
            if (result.Status == "Ok")
            {
                var listEmployee = result.Data.Select(x => (EmployeeDto)x).ToList();

                // Mengirim data karyawan ke tampilan
                ViewBag.EmployeeData = listEmployee;
                return View();
            }
            else
            {
                // Handle kesalahan jika diperlukan
                return View();
            }
        }
    }
}
