﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            if (user != null)
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
        //Get Owner/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("OwnerId", "OwnerName", "OwnerAddress", "MemberSince")] Owner owner)
        {
            if (ModelState.IsValid)
            {
                db.Owners.Add(owner);
                db.SaveChanges();
                var res = await MapUserToOwner(owner);

                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        private async Task<IActionResult> MapUserToOwner(Owner owner)
        {
            var user = await GetCurrentUserAync();
            user.owner = owner;
            var user_res = await _userManager.UpdateAsync(user);
            if(user_res == IdentityResult.Success)
            {
                Debug.WriteLine("We mapped the owner to the user");
            }
            else
            {
                Debug.WriteLine("Could not map owner to the user");
                return BadRequest(user_res);
            }
            owner.user = user;
            owner.UserID = user.Id;

            if (ModelState.IsValid)
            {
                db.Entry(owner).State = EntityState.Modified;
                var owner_res = await db.SaveChangesAsync();
                if (owner_res > 0)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest(owner_res);
                }
                
            }
            else
            {
                return BadRequest("Unstable Owner Model");
            }
        }
    }
}