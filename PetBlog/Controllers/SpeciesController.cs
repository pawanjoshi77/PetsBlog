using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PetBlog.Models;

namespace PetBlog.Controllers
{
    public class SpeciesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View(new Species());
        }

        [HttpPost]
        public IActionResult Add(Species species)
        {
            return View();
        }

        [HttpGet]
        public IActionResult Edit()
        {
            return View(new Species());
        }

        [HttpPost]
        public IActionResult Edit(Species species)
        {
            return View();
        }
    }
}