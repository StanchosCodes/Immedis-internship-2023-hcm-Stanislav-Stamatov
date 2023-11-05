﻿using HumanCapitalManagement.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace HumanCapitalManagement.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            // this.HttpContext.Session.SetString("Name", "Shit");

            return View();
        }

        public IActionResult Privacy()
        {
            //string? name = this.HttpContext.Session.GetString("Name");

            //if (!String.IsNullOrEmpty(name))
            //{
            //    return Ok(name);
            //}

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}