using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FPRMAspNetCoreMVC.Data;
using FPRMAspNetCoreMVC.Models;

namespace FPRMAspNetCoreMVC.Controllers
{
    public class RentalsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RentalsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Rentals
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Rental.Include(r => r.Apartment).Include(r => r.Tenant);

            if (User.Identity.IsAuthenticated && User.IsInRole("Administrator"))
            {
                ViewBag.ShowCreateButton = true;
                ViewBag.ShowEditButton = true;
                ViewBag.ShowDetailsButton = true;
                ViewBag.ShowDeleteButton = true;
            }
            else
            {
                ViewBag.ShowCreateButton = false;
                ViewBag.ShowEditButton = false;
                ViewBag.ShowDetailsButton = true;
                ViewBag.ShowDeleteButton = false;
            }

            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Rentals/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rental = await _context.Rental
                .Include(r => r.Apartment)
                .Include(r => r.Tenant)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rental == null)
            {
                return NotFound();
            }

            return View(rental);
        }

        // GET: Rentals/Create
        public IActionResult Create()
        {
            ViewData["ApartmentId"] = new SelectList(_context.Apartment, "Id", "Unit");
            ViewData["TenantId"] = new SelectList(_context.Set<User>(), "Id", "Name");
            return View();
        }

        // POST: Rentals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ApartmentId,TenantId,InitialRentDate,FinalRentDate,MonthlyRent,Description,Id")] Rental rental)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rental);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApartmentId"] = new SelectList(_context.Apartment, "Id", "Unit", rental.ApartmentId);
            ViewData["TenantId"] = new SelectList(_context.Set<User>(), "Id", "Name", rental.TenantId);
            return View(rental);
        }

        // GET: Rentals/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rental = await _context.Rental.FindAsync(id);
            if (rental == null)
            {
                return NotFound();
            }
            ViewData["ApartmentId"] = new SelectList(_context.Apartment, "Id", "Unit", rental.ApartmentId);
            ViewData["TenantId"] = new SelectList(_context.Set<User>(), "Id", "Name", rental.TenantId);
            return View(rental);
        }

        // POST: Rentals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ApartmentId,TenantId,InitialRentDate,FinalRentDate,MonthlyRent,Description,Id")] Rental rental)
        {
            if (id != rental.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rental);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RentalExists(rental.Id))
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
            ViewData["ApartmentId"] = new SelectList(_context.Apartment, "Id", "Unit", rental.ApartmentId);
            ViewData["TenantId"] = new SelectList(_context.Set<User>(), "Id", "Name", rental.TenantId);
            return View(rental);
        }

        // GET: Rentals/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rental = await _context.Rental
                .Include(r => r.Apartment)
                .Include(r => r.Tenant)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rental == null)
            {
                return NotFound();
            }

            return View(rental);
        }

        // POST: Rentals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var rental = await _context.Rental.FindAsync(id);
            if (rental != null)
            {
                _context.Rental.Remove(rental);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RentalExists(Guid id)
        {
            return _context.Rental.Any(e => e.Id == id);
        }
    }
}
