using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OO_Backend.Models;

namespace OO_Backend.Controllers
{
    public class DogModelsController : Controller
    {
        private readonly DatabaseContext _context;

        public DogModelsController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: DogModels
        public async Task<IActionResult> Index()
        {
            return View(await _context.Dogs.ToListAsync());
        }

        // GET: DogModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dogModel = await _context.Dogs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dogModel == null)
            {
                return NotFound();
            }

            return View(dogModel);
        }

        // GET: DogModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DogModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,OwnerId,Name,BirthDate,Breed,ImageUrl")] DogModel dogModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dogModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dogModel);
        }

        // GET: DogModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dogModel = await _context.Dogs.FindAsync(id);
            if (dogModel == null)
            {
                return NotFound();
            }
            return View(dogModel);
        }

        // POST: DogModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OwnerId,Name,BirthDate,Breed,ImageUrl")] DogModel dogModel)
        {
            if (id != dogModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dogModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DogModelExists(dogModel.Id))
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
            return View(dogModel);
        }

        // GET: DogModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dogModel = await _context.Dogs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dogModel == null)
            {
                return NotFound();
            }

            return View(dogModel);
        }

        // POST: DogModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dogModel = await _context.Dogs.FindAsync(id);
            _context.Dogs.Remove(dogModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DogModelExists(int id)
        {
            return _context.Dogs.Any(e => e.Id == id);
        }
    }
}
