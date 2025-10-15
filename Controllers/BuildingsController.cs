using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FPRMAspNetCoreMVC.Data;
using FPRMAspNetCoreMVC.Models;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FPRMAspNetCoreMVC.Controllers
{
    public class BuildingsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;


        public BuildingsController(ApplicationDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        private void SetViewBagPermissions()
        {
            if (User.Identity.IsAuthenticated && (User.IsInRole("Administrator") || User.IsInRole("Manager")))
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
        }

        // GET: Buildings
        public async Task<IActionResult> Index(string filter, bool? onlyActive)
        {
            IQueryable<Building> buildingsQuery = _context.Building.Include(b => b.Manager);

            if (!string.IsNullOrEmpty(filter))
            {
                buildingsQuery = buildingsQuery.Where(b => b.Name.Contains(filter) ||
                                                           b.Description.Contains(filter) ||
                                                           b.City.Contains(filter) ||
                                                           b.Country.Contains(filter) ||
                                                           b.Manager.Name.Contains(filter));
            }

            if (onlyActive.HasValue && onlyActive.Value)
            {
                buildingsQuery = buildingsQuery.Where(b => b.IsActive);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (User.Identity.IsAuthenticated && User.IsInRole("Manager"))
            {
                buildingsQuery = buildingsQuery.Where(b => b.ManagerId.ToString() == userId);
            }

            if (!User.Identity.IsAuthenticated || User.IsInRole("Tenant"))
            {
                buildingsQuery = buildingsQuery.Where(b => b.IsActive);
                ViewBag.ShowCreateButton = false;
                ViewBag.ShowEditButton = false;
                ViewBag.ShowDetailsButton = true;
                ViewBag.ShowDeleteButton = false;
            }
            else
            {
                SetViewBagPermissions();
            }

            var buildings = await buildingsQuery.ToListAsync();

            return View(buildings);
        }


        // GET: Buildings/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var building = await _context.Building
                .Include(b => b.Manager)
                .Include(b => b.Apartments)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (building == null)
            {
                return NotFound();
            }

            if (building.ImageData != null && building.ImageData.Length > 0)
            {
                string imageBase64 = Convert.ToBase64String(building.ImageData);
                string imageData = string.Format("data:image/jpeg;base64,{0}", imageBase64);
                ViewBag.ImageData = imageData;
            }
            else
            {
                ViewBag.ImageData = null;
            }

            return View(building);
        }

        // GET: Buildings/Create
        public IActionResult Create()
        {
            ViewData["ManagerId"] = new SelectList(_context.User.Where(u => (int)u.UserRole == 1), "Id", "Name");
            return View();
        }
        
        // POST: Buildings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Building building, IFormFile image)
        {
            if (ModelState.IsValid)
            {

                if (image != null && image.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        await image.CopyToAsync(ms);
                        building.ImageData = ms.ToArray();
                    }
                }
                else
                {
                    var webRootPath = _hostingEnvironment.WebRootPath;
                    string defaultImagePath = Path.Combine(webRootPath, "img", "no_available_img.jpeg");

                    try
                    {
                        byte[] defaultImageData = System.IO.File.ReadAllBytes(defaultImagePath);
                        building.ImageData = defaultImageData;
                    }
                    catch (Exception ex)
                    {
                        return Content("Error to select image: " + ex.Message);
                    }
                }


                _context.Add(building);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ManagerId"] = new SelectList(_context.User, "Id", "Name", building.ManagerId); 
            return View(building);
        }

        // GET: Buildings/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var building = await _context.Building
                .Include(b => b.Manager)
                .FirstOrDefaultAsync(m => m.Id == id && (int)m.Manager.UserRole == 1);

            if (building == null)
            {
                return NotFound();
            }

            if (building.ImageData != null)
            {
                string imageData = string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(building.ImageData));
                ViewBag.ImageData = imageData;
            }

            ViewData["ManagerId"] = new SelectList(_context.User.Where(u => (int)u.UserRole == 1), "Id", "Name", building.ManagerId);
            return View(building);
        }



        // POST: Buildings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Building building, IFormFile image)
        {
            if (id != building.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (image != null && image.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            await image.CopyToAsync(ms);
                            building.ImageData = ms.ToArray();
                        }
                    }
                    else
                    {
                        var webRootPath = _hostingEnvironment.WebRootPath;
                        string defaultImagePath = Path.Combine(webRootPath, "img", "no_available_img.jpeg");

                        try
                        {
                            byte[] defaultImageData = System.IO.File.ReadAllBytes(defaultImagePath);
                            building.ImageData = defaultImageData;
                        }
                        catch (Exception ex)
                        {
                            return Content("Error to select image: " + ex.Message);
                        }
                    }

                    _context.Update(building);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BuildingExists(building.Id))
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
            ViewData["ManagerId"] = new SelectList(_context.User, "Id", "Name", building.ManagerId);
            return View(building);
        }


        // GET: Buildings/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var building = await _context.Building
                .Include(b => b.Manager)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (building == null)
            {
                return NotFound();
            }

            return View(building);
        }

        // POST: Buildings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var building = await _context.Building.FindAsync(id);
            if (building != null)
            {
                _context.Building.Remove(building);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BuildingExists(Guid id)
        {
            return _context.Building.Any(e => e.Id == id);
        }

    }
}
