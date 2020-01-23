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
    public class OfferAdsController : Controller
    {
        private readonly DatabaseContext _context;

        public OfferAdsController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: OfferAds
        public async Task<IActionResult> Index()
        {
            return View(await _context.OfferServicesAds.ToListAsync());
        }

        // GET: OfferAds/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var offerAd = await _context.OfferServicesAds
                .FirstOrDefaultAsync(m => m.Id == id);
            if (offerAd == null)
            {
                return NotFound();
            }

            return View(offerAd);
        }

        // GET: OfferAds/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: OfferAds/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,Body,PostDate,DayAvailableFrom,DayAvailableTo,HourAvailableFrom,HourAvailableTo")] OfferAd offerAd)
        {
            if (ModelState.IsValid)
            {
                _context.Add(offerAd);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(offerAd);
        }

        // GET: OfferAds/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var offerAd = await _context.OfferServicesAds.FindAsync(id);
            if (offerAd == null)
            {
                return NotFound();
            }
            return View(offerAd);
        }

        // POST: OfferAds/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,Body,PostDate,DayAvailableFrom,DayAvailableTo,HourAvailableFrom,HourAvailableTo")] OfferAd offerAd)
        {
            if (id != offerAd.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(offerAd);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OfferAdExists(offerAd.Id))
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
            return View(offerAd);
        }

        // GET: OfferAds/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var offerAd = await _context.OfferServicesAds
                .FirstOrDefaultAsync(m => m.Id == id);
            if (offerAd == null)
            {
                return NotFound();
            }

            return View(offerAd);
        }

        // POST: OfferAds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var offerAd = await _context.OfferServicesAds.FindAsync(id);
            _context.OfferServicesAds.Remove(offerAd);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OfferAdExists(int id)
        {
            return _context.OfferServicesAds.Any(e => e.Id == id);
        }
    }
}
