using API.DTOs.Accounts;
using API.DTOs.Employees;
using API.DTOs.Roles;
using API.Models;
using CLIENT.Contract;
using CLIENT.Models;
using CLIENT.Repository;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;
using System.Diagnostics;
using System.Net;

namespace CLIENT.Controllers
{
    public class HomeClientController : Controller
    {

        private readonly IHomeClientRepository _homeClientRepository;

        public HomeClientController(IHomeClientRepository homeClientRepository)
        {
            _homeClientRepository = homeClientRepository;
        }

        public IActionResult HomeClient()
        {
            return View();
        }

        public IActionResult DetailHomeIdle()
        {
            return View();
        }

    }
}
