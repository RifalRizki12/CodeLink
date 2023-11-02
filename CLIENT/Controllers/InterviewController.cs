using API.DTOs.Accounts;
using API.DTOs.Employees;
using API.DTOs.Interviews;
using API.Models;
using CLIENT.Contract;
using Microsoft.AspNetCore.Authorization;
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

        [HttpGet("Interview/ListHireIdle/{guid}")]
        public async Task<JsonResult> ListHireIdle(Guid guid)
        {
            var interview = new GetInterviewDto();
            var result = await _repository.GetByCompanyGuid(guid);
            if (result.Data != null)
            {
                return Json(result.Data);
            }
            else
            {
                return Json(interview); ;
            }

        }

        [HttpGet("Interview/GetOnsite/{guid}")]
        public async Task<JsonResult> GetOnsite(Guid guid)
        {
            var interview = new GetInterviewDto();
            var result = await _repository.GetOnsite(guid);
            if (result.Data != null)
            {
                return Json(result.Data);
            }
            else
            {
                return Json(interview); ;
            }

        }

        [HttpPost("Interview/AddSchedule")]
        public async Task<JsonResult> AddSchedule([FromBody] CreateInterviewDto createDto)
        {
            var response = await _repository.Post(createDto);
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
