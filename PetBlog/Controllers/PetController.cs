using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PetBlog.Data;
using PetBlog.Models;

namespace PetBlog.Controllers
{
    public class PetController : Controller
    {
        private readonly PetsBlogContext _context;

        public PetController(PetsBlogContext context)
        {
            _context = context;
        }

        // GET: Pet
        public async Task<IActionResult> Index()
        {
            var petsBlogContext = _context.Pets.Include(p => p.Owner).Include(p => p.Species);
            return View(await petsBlogContext.ToListAsync());
        }

        // GET: Pet/Details
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pet = await _context.Pets
                .Include(p => p.Owner)
                .Include(p => p.Species)
                .SingleOrDefaultAsync(m => m.PetID == id);
            if (pet == null)
            {
                return NotFound();
            }

            return View(pet);
        }

        // GET: Pet/Create
        public IActionResult Create()
        {
            ViewData["OwnerID"] = new SelectList(_context.Owners, "OwnerID", "OwnerAddress");
            ViewData["PetID"] = new SelectList(_context.Species, "SpeciesID", "SpeciesGender");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PetName, PetType, PetSize, PetDOB, PetGender, HasPic, ImgType, SpeciesID, OwnerID")] Pet pet)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OwnerID"] = new SelectList(_context.Owners, "OwnerID", "OwnerAddress", pet.OwnerID);
            ViewData["PetID"] = new SelectList(_context.Species, "SpeciesID", "SpeciesGender", pet.PetID);
            return View(pet);
        }

        // GET: Pet/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pet = await _context.Pets.SingleOrDefaultAsync(m => m.PetID == id);
            if (pet == null)
            {
                return NotFound();
            }
            ViewData["OwnerID"] = new SelectList(_context.Owners, "OwnerID", "OwnerAddress", pet.OwnerID);
            ViewData["PetID"] = new SelectList(_context.Species, "SpeciesID", "SpeciesGender", pet.PetID);
            return View(pet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PetName, PetType, PetSize, PetDOB, PetGender, HasPic, ImgType, SpeciesID, OwnerID")] Pet pet)
        {
            if (id != pet.PetID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PetExists(pet.PetID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["OwnerID"] = new SelectList(_context.Owners, "OwnerID", "OwnerAddress", pet.OwnerID);
            ViewData["PetID"] = new SelectList(_context.Species, "SpeciesID", "SpeciesGender", pet.PetID);
            return View(pet);
        }

        // GET: Pet/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pet = await _context.Pets
                .Include(p => p.Owner)
                .Include(p => p.Species)
                .SingleOrDefaultAsync(m => m.PetID == id);
            if (pet == null)
            {
                return NotFound();
            }

            return View(pet);
        }

        // POST: Pet/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pet = await _context.Pets.SingleOrDefaultAsync(m => m.PetID == id);
            _context.Pets.Remove(pet);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PetExists(int id)
        {
            return _context.Pets.Any(e => e.PetID == id);
        }
    }
}
