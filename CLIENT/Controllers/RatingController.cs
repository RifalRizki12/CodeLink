using API.DTOs.Accounts;
using API.DTOs.Employees;
using API.DTOs.Ratings;
using API.Models;
using CLIENT.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;


namespace CLIENT.Controllers
{
    public class RatingController : Controller
    {

        private readonly IRatingRepository _repository;

        public RatingController(IRatingRepository repository)
        {
            _repository = repository;
        }




        [HttpPut]
        public async Task<JsonResult> UpdateRating([FromBody] RatingDto rating)
        {
            var response = await _repository.Put(rating.Guid, rating);

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
