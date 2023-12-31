﻿using API.DTOs.Accounts;
using API.DTOs.Employees;
using API.DTOs.Roles;
using API.Models;
using CLIENT.Contract;
using CLIENT.Models;
using CLIENT.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;
using System.Diagnostics;
using System.Net;

namespace CLIENT.Controllers
{
    [Authorize]
    public class HomeClientController : Controller
    {

        private readonly IHomeClientRepository _homeClientRepository;

        public HomeClientController(IHomeClientRepository homeClientRepository)
        {
            _homeClientRepository = homeClientRepository;
        }

        [AllowAnonymous]
        public IActionResult HomeClient()
        {
            return View();
        }

        public IActionResult DetailHomeIdle()
        {
            return View();
        }

        [Authorize(Roles = "client")]
        public IActionResult ListHireIdle()
        {
            return View();
        }
        
        public IActionResult MyAccount()
        {
            return View();
        }

    }
}
