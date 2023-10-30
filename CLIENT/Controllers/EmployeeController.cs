using API.DTOs.Accounts;
using API.DTOs.Employees;
using API.Utilities.Handler;
using CLIENT.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Protocol.Core.Types;
using System.Collections.Generic;
using System.Net;

namespace CLIENT.Controllers
{
    /*[Authorize]*/
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository repository;

        public EmployeeController(IEmployeeRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<JsonResult> GetEmployeeData()
        {
            var result = await repository.GetDetailIdle();
            var listEmployee = new List<EmployeeDetailDto>();
            if (result != null)
            {
                listEmployee = result.Data.Select(x => (EmployeeDetailDto)x).ToList();
            }
            return Json(new { data = result.Data });
        }

        public async Task<IActionResult> RegisterIdle()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterIdle([FromForm] RegisterIdleDto registrationDto)
        {
            if (ModelState.IsValid)
            {
                // Anda sekarang bisa mengakses registrationDto.Skills sebagai List<string>
                var skills = registrationDto.Skills;
                var result = await repository.RegisterIdle(registrationDto);

                if (result.Status == "OK")
                {
                    // Pendaftaran berhasil
                    return Json(new { success = true, redirectTo = Url.Action("Index", "Employee") });
                }
                else
                {
                    // Pendaftaran gagal atau ada kesalahan
                    return Json(new { success = false, message = "Pendaftaran gagal atau terjadi kesalahan." });
                }
            }
            else
            {
                // Data yang dikirim tidak valid
                return Json(new { success = false, message = "Data tidak valid." });
            }
        }

        public async Task<IActionResult> RegisterClient()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterClient([FromForm] RegisterClientDto registrationCDto)
        {
            if (ModelState.IsValid)
            {
                // Anda sekarang bisa mengakses registrationDto.Skills sebagai List<string>

                var result = await repository.RegisterClient(registrationCDto);

                if (result.Status == "OK")
                {
                    // Pendaftaran berhasil
                    return Json(new { success = true, redirectTo = Url.Action("Index", "Employee") });
                }
                else
                {
                    // Pendaftaran gagal atau ada kesalahan
                    return Json(new { success = false, message = "Pendaftaran gagal atau terjadi kesalahan." });
                }
            }
            else
            {
                // Data yang dikirim tidak valid
                return Json(new { success = false, message = "Data tidak valid." });
            }
        }




        [HttpPut("updateIdle")]
        public async Task<JsonResult> UpdateIdle(UpdateIdleDto employeeDto)
        {
            var response = await repository.UpdateIdle(employeeDto);

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




        public async Task<IActionResult> GetClient()
        {
            return View();
        }


        [HttpGet]
        public async Task<JsonResult> GetClientData()
        {
            var result = await repository.GetDetailClient();
            return Json(new { data = result.Data });
        }


        [HttpGet("Employee/GetGuidClient/{guid}")]
        public async Task<JsonResult> GetGuidClient(Guid guid)
        {
            var result = await repository.GetGuidClient(guid);
            var employee = new AccountDto();

            if (result.Data?.CompanyGuid != null)
            {
                return Json(result.Data);
            }
            else
            {
                return Json(employee);
            }
        }

        [HttpGet("Employee/GetGuidClient/{guid}")]
        public async Task<JsonResult> GetGuidEmployee(Guid guid)
        {
            var result = await repository.GetGuidEmployee(guid);
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

        [HttpPut]
        public async Task<IActionResult> UpdateClient([FromForm] UpdateClientDto updateDto)
        {
            if (ModelState.IsValid)
            {
                // Anda sekarang bisa mengakses updateDto.Skills sebagai List<string>

                var result = await repository.UpdateClient(updateDto);

                if (result.Status == "OK")
                {
                    // Pembaruan berhasil
                    return Json(new { success = true, redirectTo = Url.Action("Index", "Employee") });
                }
                else
                {
                    // Pembaruan gagal atau ada kesalahan
                    return Json(new { success = false, message = "Pembaruan gagal atau terjadi kesalahan." });
                }
            }
            else
            {
                // Data yang dikirim tidak valid
                return Json(new { success = false, message = "Data tidak valid." });
            }
        }

        /*[HttpPut]
        public async Task<JsonResult> UpdateClient([FromForm] UpdateClientDto updateDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Ambil Guid langsung dari updateDto
                    var response = await repository.UpdateClient(updateDto);

                    if (response != null)
                    {
                        if (response.Status == "OK")
                        {
                            return Json(new { data = response.Data });
                        }
                        else
                        {
                            // Catat pesan kesalahan dari respons jika perlu
                            // Log.Error($"Error from repository: {response.Message}");
                            return Json(new { error = response.Message });
                        }
                    }
                    else
                    {
                        // Catat kejadian ini sebagai potensi masalah
                        // Log.Warn("Repository response was null when updating client.");
                        return Json(new { error = "Terjadi kesalahan saat memperbarui data karyawan." });
                    }
                }
                else
                {
                    // Catat model yang tidak valid jika perlu untuk debugging
                    // Log.Warn($"Invalid model data received: {JsonConvert.SerializeObject(updateDto)}");
                    return Json(new { success = false, message = "Data tidak valid." });
                }
            }
            catch (Exception ex)
            {
                // Catat eksepsi untuk analisis lebih lanjut
                // Log.Error($"Exception occurred while updating client: {ex.Message}", ex);
                return Json(new { error = $"Terjadi eksepsi: {ex.Message}" });
            }
        }*/



        public IActionResult GetChart()
        {
            // Pastikan Anda memasukkan data yang diperlukan ke dalam View jika diperlukan
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetChartData()
        {
            try
            {
                var result = await repository.GetDetailChart();
                return Json(new { data = result.Data });
            }
            catch (Exception ex)
            {
                // Tangani eksepsi yang mungkin terjadi
                return Json(new { error = ex.Message });
            }
        }

    }
}
