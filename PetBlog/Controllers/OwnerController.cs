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

        private async Task<ApplicationUser> GetCurrentUserAsync() => await _userManager.GetUserAsync(HttpContext.User);

        public OwnerController(PetsBlogContext context, IHostingEnvironment env, UserManager<ApplicationUser> usermanager)
        {
            db = context;
            _env = env;
            _userManager = usermanager;
        }

        public async Task<int> GetUserDetails(ApplicationUser user)
        {
            if (user == null)
            {
                return 0;
            }
            var userid = user.Id;
            if (user.OwnerID == null) return 1;
            else
            {
                return 2;
            }
        }

        //GET: Owners
        public async Task<ActionResult> Index()
        {
            var user = await GetCurrentUserAsync();
            var userstate = await GetUserDetails(user);

            ViewData["UserState"] = userstate;
            ViewData["UserOwnerID"] = 0; //default to 0
            if (userstate == 2)
            {
                ViewData["UserOwnerID"] = user.OwnerID;

            }
            return View(await db.Owners.ToListAsync());
        }

        public ActionResult Show(int id)
        {
            //wrapper function. Show will redirect to details.
            return RedirectToAction("Details/" + id);
        }

        public async Task<ActionResult> Details(int? id)
        {
            //get the current user id for now (proof of concept).
            var user = await GetCurrentUserAsync();
            var userstate = await GetUserDetails(user);
            ViewData["UserOwnerID"] = 0;
            if (userstate == 2) ViewData["UserOwnerID"] = user.OwnerID;

            Owner located_owner = await db.Owners.Include(o => o.Pets).SingleOrDefaultAsync(o => o.OwnerID == id);
            if (located_owner == null)
            {
                return NotFound();
            }
            //Check the current user's owner against the owner.
            //Debug.WriteLine("Asked for user in details. The user is " + user.Id.ToString());
            //Check what the located owner id is
            //Debug.WriteLine("Asked for located owner in details. The located owner is" + located_owner.OwnerID);
            //This one only works when we pick msg as the owner
            //Debug.WriteLine("Asked for user's owner in details. The user is " + user.owner.ToString());
            

            ViewData["UserState"] = userstate;
            return View(located_owner);
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
            //can I find out if someone is logged in at the time this command is executed?
            //if not, I don't actually want to create an owner..
            //if yes, then can I find that user id?
            //if there is a user and I know the id,
            //all I need to do is map the owner to the user
             var user = await GetCurrentUserAsync();
            //user.OwnerID = set this value
            //owner.UserID = set this value

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
            var user = await GetCurrentUserAsync();
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
            var user = await GetCurrentUserAsync();
            if (user == null) return Forbid();
            if (user.OwnerID != id)
            {
                return Forbid();
            }
            return View(owner);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind("OwnerID", "OwnerName", "OwnerAddress", "MemberSince")] Owner owner)
        {
            var user = await GetCurrentUserAsync();
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

        public async Task<ActionResult> Delete(int id)
        {
            
            var owner = await db.Owners.FindAsync(id);
            if (owner == null)
            {
                return NotFound();
            }
            var user = await GetCurrentUserAsync();
            var userstate = await GetUserDetails(user);
            if(userstate == 2)
            {
                if (id != user.OwnerID) return Forbid();
                return View(owner);
            }
            return NotFound();
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Owner owner = await db.Owners.FindAsync(id);
            var user = await GetCurrentUserAsync();
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
                    var user = await GetCurrentUserAsync();
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
