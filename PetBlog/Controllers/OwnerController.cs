using System;
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
        // view details when clicked in owners tab
        [HttpGet]
        public ActionResult Details(int id)
        {
            //view the details of the owner that you clicked
            //get the information about the owner

            
            //passing this information to Views/Owner/Details.cshtml
            return View(db.Owners.Find(id));
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

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new StatusCodeResult(400);
            }
            Owner owner = db.Owners.Find(id);
            if (owner == null)
            {
                return NotFound();
            }
            var user = await GetCurrentUserAync();
            if (user == null) return Forbid();
            if (user.OwnerID != id)
            {
                return Forbid();
            }
            return View(owner);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind("OwnerId", "OwnerName", "OwnerAddress", "MemberSince")] Owner owner)
        {
            var user = await GetCurrentUserAync();
            if (user == null) return Forbid();
            if (user.OwnerID != owner.OwnerID)
            {
                return Forbid();
            }
            if (ModelState.IsValid)
            {
                db.Entry(owner).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(owner);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new StatusCodeResult(400);
            }
            var owner = db.Owners.FindAsync(id);
            if (owner == null)
            {
                return NotFound();
            }
            var user = await GetCurrentUserAync();
            if (user == null)
            {
                return Forbid();
            }
            if (user.OwnerID != id)
            {
                return Forbid();
            }
            return View(owner);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Owner owner = await db.Owners.FindAsync(id);
            var user = await GetCurrentUserAync();
            if (user.OwnerID != id)

            {
                return Forbid();
            }
            await UnmapUserFromOwner(id);
            db.Owners.Remove(owner);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> UnmapUserFromOwner(int id)

        {
            Owner owner = await db.Owners.FindAsync(id);
            owner.user = null;
            owner.UserID = "";
            if (ModelState.IsValid)
            {
                db.Entry(owner).State = EntityState.Modified;
                var owner_res = await db.SaveChangesAsync();
                if (owner_res == 0)
                {
                    return BadRequest(owner_res);
                }
                else
                {
                    var user = await GetCurrentUserAync();
                    user.owner = null;
                    user.OwnerID = null;
                    var user_res = await _userManager.UpdateAsync(user);
                    if (user_res == IdentityResult.Success)
                    {
                        Debug.WriteLine("User Updated");
                        return Ok();
                    }
                    else
                    {
                        Debug.WriteLine("Not able to update the user");
                        return BadRequest(user_res);
                    }
                }
            }

            else
            {
                return BadRequest("Unstable model");
            }
        }
     }
}
