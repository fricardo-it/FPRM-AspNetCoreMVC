using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FPRMAspNetCoreMVC.Data;
using FPRMAspNetCoreMVC.Models;
using System.Security.Claims;

namespace FPRMAspNetCoreMVC.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AppointmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Appointments
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Appointment.Include(a => a.Building).Include(a => a.Tenant);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Appointments/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointment
                .Include(a => a.Building)
                .Include(a => a.Tenant)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // GET: Appointments/Create
        public IActionResult Create()
        {
            ViewData["BuildingId"] = new SelectList(_context.Building, "Id", "Name");
            ViewData["TenantId"] = new SelectList(_context.User, "Id", "Name");
            return View();
        }

        // POST: Appointments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TenantId,BuildingId,AppointmentDate,Message,IsConfirmed,Id")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(appointment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BuildingId"] = new SelectList(_context.Building, "Id", "Name", appointment.BuildingId);
            ViewData["TenantId"] = new SelectList(_context.User, "Id", "Name", appointment.TenantId);
            return View(appointment);
        }

        // GET: Appointments/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointment.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }
            ViewData["BuildingId"] = new SelectList(_context.Building, "Id", "Name", appointment.BuildingId);
            ViewData["TenantId"] = new SelectList(_context.User, "Id", "Name", appointment.TenantId);
            return View(appointment);
        }

        // POST: Appointments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("TenantId,BuildingId,AppointmentDate,Message,IsConfirmed,Id")] Appointment appointment)
        {
            if (id != appointment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appointment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentExists(appointment.Id))
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
            ViewData["BuildingId"] = new SelectList(_context.Building, "Id", "Name", appointment.BuildingId);
            ViewData["TenantId"] = new SelectList(_context.User, "Id", "Name", appointment.TenantId);
            return View(appointment);
        }

        // GET: Appointments/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointment
                .Include(a => a.Building)
                .Include(a => a.Tenant)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var appointment = await _context.Appointment.FindAsync(id);
            if (appointment != null)
            {
                _context.Appointment.Remove(appointment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppointmentExists(Guid id)
        {
            return _context.Appointment.Any(e => e.Id == id);
        }

        private List<SelectListItem> GetTypeApartmentSelectList(string selectedType)
        {
            var typeApartmentList = new List<SelectListItem>
            {
                new SelectListItem { Value = "Basement", Text = "Basement" },
                new SelectListItem { Value = "1 1/2", Text = "1 1/2" },
                new SelectListItem { Value = "2 1/2", Text = "2 1/2" },
                new SelectListItem { Value = "3 1/2", Text = "3 1/2" },
                new SelectListItem { Value = "4 1/2", Text = "4 1/2" },
                new SelectListItem { Value = "5 1/2", Text = "5 1/2" },
                new SelectListItem { Value = "Penthouse", Text = "Penthouse" },
                new SelectListItem { Value = "Loft", Text = "Loft" },
                new SelectListItem { Value = "Duplex", Text = "Duplex" },
                new SelectListItem { Value = "Triplex", Text = "Triplex" }
            };

            if (!string.IsNullOrEmpty(selectedType))
            {
                typeApartmentList.ForEach(x => x.Selected = x.Value == selectedType);
            }

            return typeApartmentList;
        }

        // GET: Appointments/CreateForTenant
        public async Task<IActionResult> CreateForTenant()
        {
            if (User.Identity.IsAuthenticated && User.IsInRole("Tenant"))
            {
                var buildings = await _context.Building.ToListAsync();

                if (buildings.Any())
                {
                    var selectedBuildingId = buildings.First().Id;

                    var apartments = await _context.Apartment.Where(a => a.BuildingId == selectedBuildingId).ToListAsync();

                    ViewBag.BuildingId = new SelectList(buildings, "Id", "Name", selectedBuildingId);
                    ViewBag.ApartmentList = new SelectList(apartments, "Id", "Unit");

                    return View();
                }
                else
                {
                    return RedirectToAction("NoBuildingsFound");
                }
            }
            else
            {
                return RedirectToAction("AccessDenied");
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateForTenant(Appointment appointment, Guid selectedBuildingId)
        {
            if (User.Identity.IsAuthenticated && User.IsInRole("Tenant"))
            {
                var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (ModelState.IsValid)
                {
                    var buildingId = selectedBuildingId;

                    if (buildingId != null)
                    {
                        var building = await _context.Building.FindAsync(buildingId);

                        if (building != null)
                        {
                            var managerId = building.ManagerId ?? Guid.Empty;

                            appointment.ManagerId = managerId;

                            if (Guid.TryParse(userIdString, out Guid userId))
                            {
                                appointment.TenantId = userId;
                                appointment.BuildingId = building.Id;
                                appointment.IsConfirmed = false;

                                _context.Add(appointment);
                                await _context.SaveChangesAsync();

                                return RedirectToAction(nameof(Index));
                            }
                            else
                            {
                                return RedirectToAction("Error");
                            }
                        }
                        else
                        {
                            return RedirectToAction("BuildingNotFound");
                        }
                    }
                    else
                    {
                        return RedirectToAction("BuildingIdIsNull");
                    }
                }

                ViewBag.BuildingId = new SelectList(_context.Building, "Id", "Name", appointment.BuildingId);
                return View(appointment);
            }
            else
            {
                return RedirectToAction("AccessDenied");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetApartmentsForBuilding(Guid buildingId)
        {
            var apartments = await _context.Apartment.Where(a => a.BuildingId == buildingId).ToListAsync();
            var apartmentList = apartments.Select(a => new { value = a.Id, text = a.Unit }).ToList();
            return Json(apartmentList);
        }



        public async Task<IActionResult> MyAppointments()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId != null)
                {
                    if (Guid.TryParse(userId, out Guid userIdGuid))
                    {
                        var userAppointments = await _context.Appointment
                            .Include(a => a.Building)
                            .ThenInclude(b => b.Apartments)
                            .Include(a => a.Tenant)
                            .Where(a => a.TenantId == userIdGuid)
                            .ToListAsync();

                        return View(userAppointments);
                    }
                    else
                    {
                        return RedirectToAction("AccessDenied");
                    }
                }
            }

            return RedirectToAction("AccessDenied");
        }

        // GET: Appointments/PendingRequests
        public async Task<IActionResult> PendingRequests()
        {
            if (User.Identity.IsAuthenticated && User.IsInRole("Manager"))
            {
                var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (Guid.TryParse(userIdString, out Guid managerId))
                {
                    var pendingRequests = await _context.Appointment
                        .Include(a => a.Tenant)
                        .Include(a => a.Building)
                        .Where(a => a.Building.ManagerId == managerId && !a.IsConfirmed)
                        .ToListAsync();

                    return View(pendingRequests);
                }
            }

            return RedirectToAction("AccessDenied");
        }

        public async Task<IActionResult> Approve(Guid? id)
        {
            var appointment = await _context.Appointment.FindAsync(id);
            if (appointment != null)
            {
                appointment.IsConfirmed = true;
                _context.Update(appointment);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(PendingRequests));
        }

        public async Task<IActionResult> Reject(Guid id)
        {
            var appointment = await _context.Appointment.FindAsync(id);
            if (appointment != null)
            {
                _context.Appointment.Remove(appointment);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(PendingRequests));
        }

        public async Task<IActionResult> ManagerAppointments()
        {
            if (User.Identity.IsAuthenticated && User.IsInRole("Manager"))
            {
                var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (Guid.TryParse(userIdString, out Guid managerId))
                {
                    var managerAppointments = await _context.Appointment
                        .Include(a => a.Tenant)
                        .Include(a => a.Building)
                        .Where(a => a.Building.ManagerId == managerId)
                        .ToListAsync();

                    return View(managerAppointments);
                }
            }

            return RedirectToAction("AccessDenied");
        }


    }
}
