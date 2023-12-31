﻿using API.DTOs.Accounts;
using API.DTOs.Employees;
using API.Models;
using API.Utilities.Handler;
using CLIENT.Contract;
using CLIENT.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Protocol.Core.Types;
using System.Collections.Generic;
using System.Net;

namespace CLIENT.Controllers
{
    [Authorize(Roles = "admin")]
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
        public async Task<IActionResult> GetGrafik()
        {
            return View();
        }

        [AllowAnonymous]
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
            var result = await repository.RegisterIdle(registrationDto);

            if (result is ResponseOKHandler<Employee> successResult)
            {
                // Pendaftaran berhasil
                return Json(new { status = "OK", message = successResult });
            }
            else if (result is ResponseErrorHandler errorResult)
            {
                // Pendaftaran gagal atau ada kesalahan
                return Json(new { status = "Error", message = errorResult });
            }

            return Json(new { success = false, message = "Data tidak valid." });
        }

        [AllowAnonymous]
        public async Task<IActionResult> RegisterClient()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterClient([FromForm] RegisterClientDto registrationCDto)
        {
            var result = await repository.RegisterClient(registrationCDto);

            if (result is ResponseOKHandler<Company> successResult)
            {
                // Pendaftaran berhasil
                return Json(new { status = "OK", message = successResult });
            }
            else if (result is ResponseErrorHandler errorResult)
            {
                // Pendaftaran gagal atau ada kesalahan
                return Json(new { status = "Error", message = errorResult });
            }

            return Json(new { success = false, message = "Data tidak valid." });

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

        [AllowAnonymous]
        [HttpGet("Employee/GetGuidClient/{guid}")]
        public async Task<JsonResult> GetGuidClient(Guid guid)
        {
            var result = await repository.GetGuidClient(guid);
            var employee = new ClientDetailDto();

            if (result.Data?.CompanyGuid != null)
            {
                return Json(result.Data);
            }
            else
            {
                return Json(employee);
            }
        }

        [AllowAnonymous]
        [HttpGet("Employee/GetGuidEmployee/{guid}")]
        public async Task<JsonResult> GetGuidEmployee(Guid guid)
        {
            var result = await repository.GetGuidEmployee(guid);
            var employee = new EmployeeDetailDto();

            if (result.Data?.Guid != null)
            {
                return Json(result.Data);
            }
            else
            {
                return Json(employee);
            }
        }

        [AllowAnonymous]
        [HttpPut]
        public async Task<IActionResult> UpdateClient([FromForm] UpdateClientDto updateDto)
        {
            // Anda sekarang bisa mengakses updateDto.Skills sebagai List<string>

            var result = await repository.UpdateClient(updateDto);

            if (result is ResponseOKHandler<Company> successResult)
            {
                // Pembaruan berhasil
                return Json(new { status = "OK", message = successResult });
            }
            else if (result is ResponseErrorHandler errorResult)
            {
                // Pembaruan gagal atau ada kesalahan
                return Json(new { status = "Error", message = errorResult });
            }
            return Json(new { success = false, message = "Data tidak valid." });
        }

        [AllowAnonymous]
        [HttpPut]
        public async Task<IActionResult> UpdateIdle([FromForm] UpdateIdleDto updateDto)
        {
            if (ModelState.IsValid)
            {
                // Anda sekarang bisa mengakses updateDto.Skills sebagai List<string>

                var result = await repository.UpdateIdle(updateDto);

                if (result is ResponseOKHandler<Employee> successResult)
                {
                    // Pembaruan berhasil
                    return Json(new { status = "OK", message = successResult });
                }
                else if (result is ResponseErrorHandler errorResult)
                {
                    // Pembaruan gagal atau ada kesalahan
                    return Json(new { status = "Error", message = errorResult });
                }
            }
            return Json(new { success = false, message = "Data tidak valid." });
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

        public async Task<IActionResult> DetailIdle()
        {
            return View();
        }

    }
}
