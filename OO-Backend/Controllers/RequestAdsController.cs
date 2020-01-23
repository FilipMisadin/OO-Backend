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
    public class RequestAdsController : Controller
    {
        private readonly DatabaseContext _context;

        public RequestAdsController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: RequestAds
        public async Task<IActionResult> Index()
        {
            return View(await _context.RequestServicesAds.ToListAsync());
        }

        // GET: RequestAds/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requestAd = await _context.RequestServicesAds
                .FirstOrDefaultAsync(m => m.Id == id);
            if (requestAd == null)
            {
                return NotFound();
            }

            return View(requestAd);
        }

        // GET: RequestAds/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RequestAds/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,Neighborhood,Body,PostDate,MeetDate,HourFrom,HourTo,DogId")] RequestAd requestAd)
        {
            if (ModelState.IsValid)
            {
                _context.Add(requestAd);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(requestAd);
        }

        // GET: RequestAds/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requestAd = await _context.RequestServicesAds.FindAsync(id);
            if (requestAd == null)
            {
                return NotFound();
            }
            return View(requestAd);
        }

        // POST: RequestAds/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,Neighborhood,Body,PostDate,MeetDate,HourFrom,HourTo,DogId")] RequestAd requestAd)
        {
            if (id != requestAd.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(requestAd);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RequestAdExists(requestAd.Id))
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
            return View(requestAd);
        }

        // GET: RequestAds/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requestAd = await _context.RequestServicesAds
                .FirstOrDefaultAsync(m => m.Id == id);
            if (requestAd == null)
            {
                return NotFound();
            }

            return View(requestAd);
        }

        // POST: RequestAds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var requestAd = await _context.RequestServicesAds.FindAsync(id);
            _context.RequestServicesAds.Remove(requestAd);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RequestAdExists(int id)
        {
            return _context.RequestServicesAds.Any(e => e.Id == id);
        }
    }
}
