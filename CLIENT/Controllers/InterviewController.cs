﻿using API.DTOs.Accounts;
using API.DTOs.Interviews;
using CLIENT.Contract;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;


namespace CLIENT.Controllers
{
    public class InterviewController : Controller
    {

        private readonly IInterviewRepository _repository;

        public InterviewController(IInterviewRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> InterviewData()
        {
            var result = await _repository.GetAllInterview();
            return Json(new { data = result.Data });
        }

        [HttpGet("Interview/InterviewById/{guid}")]
        public async Task<JsonResult> InterviewById(Guid guid)
        {
            var result = await _repository.Get(guid);
            if (result.Data?.Guid != null)
            {
                return Json(result.Data);
            }
            else
            {
                return Json(new { error = result.Message });
            }
        }
        /*    [HttpPut]
            public async Task<JsonResult> ScheduleUpdate(ScheduleInterviewDto scheduleUpdate)
            {
                try
                {
                    var result = await _repository.ScheduleUpdate(scheduleUpdate);

                    if (result.Status == "OK")
                    {
                        return Json(new { success = true });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Pembaruan gagal: " + result.Message });
                    }
                }
                catch (Exception ex)
                {
                    // Log pesan kesalahan atau tangani kesalahan sesuai kebutuhan Anda
                    return Json(new { success = false, message = "Terjadi kesalahan saat memproses pembaruan: " + ex.Message });
                }
            }*/

        [HttpPut("Interview/ScheduleUpdate/{guid}")]
        public async Task<JsonResult> ScheduleUpdate(Guid guid, [FromBody] ScheduleInterviewDto scheduleUpdate)
        {
            var response = await _repository.ScheduleUpdate(guid, scheduleUpdate);

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
        public IActionResult ListHireIdle()
        {
            return View();
        }

        [HttpGet("GetAllByClientGuid/{companyGuid}")]
        public async Task<JsonResult> ListHireIdle(Guid guid)
        {
            var result = await _repository.Get(guid);
            if (result.Data?.Guid != null)
            {
                return Json(result.Data);
            }
            else
            {
                return Json(new { error = result.Message });
            }

        }
    }
}
