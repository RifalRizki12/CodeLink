using API.DTOs.Accounts;
using API.DTOs.Employees;
using API.Utilities.Handler;
using CLIENT.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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



/*
        [HttpPut("updateClient")]
        public async Task<JsonResult> UpdateClient(UpdateClientDto updateDto)
        {
            var response = await repository.UpdateClient(updateDto);

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
        }*/


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

        [HttpGet]
        public async Task<IActionResult> GuidClient(Guid guid)
        {
            var result = await repository.Get(guid);
            var employee = new ClientDetailDto();
            if (result.Data is null)
            {
                return View(employee);
            }
            return View(result.Data);
        }


        /* [HttpPost]*/

        /* public async Task<IActionResult> Edit(UpdateClientDto clientDto)
         {
             if (ModelState.IsValid)
             {
                 var result = await repository.Put(clientDto.AccountGuid, clientDto);
                 if (result != null)
                 {
                     if (result.Code == 200) // Perubahan berhasil
                     {
                         return RedirectToAction(nameof(Index));
                     }
                     else if (result.Code == 409) // Konflik, misalnya ada entitas dengan ID yang sama
                     {
                         ModelState.AddModelError(string.Empty, result.Message);
                         return View();
                     }
                     else
                     {
                         // Handle status kode lain sesuai kebutuhan Anda
                         // Contoh:
                         ModelState.AddModelError(string.Empty, "Terjadi kesalahan saat menyimpan perubahan.");
                         return View();
                     }
                 }
                 else
                 {
                     // Handle ketika result adalah null, misalnya ada kesalahan saat melakukan permintaan HTTP
                     ModelState.AddModelError(string.Empty, "Terjadi kesalahan saat menyimpan perubahan.");
                     return View();
                 }
             }
             return View();
         }*/
    }
}
