using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetBlog.Data;
using PetBlog.Models;

namespace PetBlog.Controllers
{
    public class OwnerController : Controller
    {
        private readonly PetsBlogContext db;
        private readonly IHostingEnvironment _env;

        private readonly UserManager<ApplicationUser> _userManager;

        private async Task<ApplicationUser> GetCurrentUserAync() => await _userManager.GetUserAsync(HttpContext.User);

        public OwnerController(PetsBlogContext context, IHostingEnvironment env, UserManager<ApplicationUser> usermanager)
        {
            db = context;
            _env = env;
            _userManager = usermanager;
        }

        //GET: Owners
        public async Task<ActionResult> Index()
        {
            var user = await GetCurrentUserAync();
            if(user != null)
            {
                if (user.OwnerID == null) { ViewData["UserHasOwner"] = "False"; }
                else { ViewData["UserHasOwner"] = user.OwnerID.ToString(); }
                return View(await db.Owners.ToListAsync());
            }
            else
            {
                ViewData["UserhasOwner"] = "None";
                return View(await db.Owners.ToListAsync());
            }
        }
    }
}