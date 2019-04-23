using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PetBlog.Models;
using PetBlog.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;

namespace PetBlog.Controllers
{
    public class PetController : Controller
    {
        private readonly PetsBlogContext db;
        private readonly IHostingEnvironment _env;

        private readonly UserManager<ApplicationUser> _userManager;

        private async Task<ApplicationUser> GetCurrentUserAync() => await _userManager.GetUserAsync(HttpContext.User);

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View(new Pet());
        }

        [HttpPost]
        public IActionResult Add(Pet pet)
        {
            return View();
        }

        [HttpGet]
        public IActionResult Edit()
        {
            return View(new Pet());
        }

        [HttpPost]
        public IActionResult Edit(Pet pet)
        {
            return View();
        }
    }
}