using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FPRMAspNetCoreMVC.Data;
using FPRMAspNetCoreMVC.Models;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.AspNetCore.Hosting;

namespace FPRMAspNetCoreMVC.Controllers
{
    public class ApartmentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;


        public ApartmentsController(IWebHostEnvironment hostingEnvironment, ApplicationDbContext context)
        {
            _hostingEnvironment = hostingEnvironment;
            _context = context;
        }


        public async Task<IActionResult> Index(string filter, bool? onlyAvailable)
        {
            IQueryable<Apartment> apartmentsQuery = _context.Apartment.Include(a => a.Building);

            if (!string.IsNullOrEmpty(filter))
            {
                apartmentsQuery = apartmentsQuery.Where(a =>
                    a.Unit.Contains(filter) ||
                    a.Description.Contains(filter) ||
                    a.Building.Name.Contains(filter) ||
                    a.Building.City.Contains(filter) ||
                    a.Building.Country.Contains(filter) ||
                    a.Bathrooms.ToString().Contains(filter) ||
                    a.Area.ToString().Contains(filter) ||
                    a.TypeApartment.Contains(filter) ||
                    a.RentAmount.ToString().Contains(filter));
            }

            if (onlyAvailable.HasValue && onlyAvailable.Value)
            {
                apartmentsQuery = apartmentsQuery.Where(a => a.IsAvailable);
            }

            if (!User.Identity.IsAuthenticated || User.IsInRole("Tenant"))
            {
                apartmentsQuery = apartmentsQuery.Where(a => a.IsAvailable && a.Building.IsActive);
                ViewBag.ShowCreateButton = false;
                ViewBag.ShowEditButton = false;
                ViewBag.ShowDetailsButton = true;
                ViewBag.ShowDeleteButton = false;
            }
            else
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null)
                {
                    return NotFound();
                }

                if (User.IsInRole("Manager"))
                {
                    apartmentsQuery = apartmentsQuery.Where(a => a.Building.ManagerId.ToString() == userId);
                }
                //else if (User.IsInRole("Tenant"))
                //{
                //    apartmentsQuery = apartmentsQuery.Where(a => a.Building.IsActive && a.IsAvailable);
                //}

                SetViewBagPermissions();
            }

            var apartments = await apartmentsQuery.ToListAsync();

            return View(apartments);
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

        // GET: Apartments/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var apartment = await _context.Apartment
                .Include(a => a.Building)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (apartment == null)
            {
                return NotFound();
            }

            if (apartment.ImageData != null && apartment.ImageData.Length > 0)
            {
                string imageBase64 = Convert.ToBase64String(apartment.ImageData);
                string imageData = string.Format("data:image/jpeg;base64,{0}", imageBase64);
                ViewBag.ImageData = imageData;
            }
            else
            {
                ViewBag.ImageUrl = null;
            }

            return View(apartment);
        }

        // GET: Apartments/Create
        public IActionResult Create()
        {
            ViewBag.BuildingId = new SelectList(_context.Building, "Id", "Name");
            ViewBag.TypeApartmentList = GetTypeApartmentSelectList(null);
            return View();
        }

        // POST: Apartments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Apartment apartment, IFormFile image)
        {
            if (ModelState.IsValid)
            {

                if (image != null && image.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        await image.CopyToAsync(ms);
                        apartment.ImageData = ms.ToArray();
                    }
                }
                else
                {
                    var webRootPath = _hostingEnvironment.WebRootPath;
                    string defaultImagePath = Path.Combine(webRootPath, "img", "no_available_img.jpeg");

                    try
                    {
                        byte[] defaultImageData = System.IO.File.ReadAllBytes(defaultImagePath);
                        apartment.ImageData = defaultImageData;
                    }
                    catch (Exception ex)
                    {
                        return Content("Erro to select image: " + ex.Message);
                    }
                }

                _context.Add(apartment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.BuildingId = new SelectList(_context.Building, "Id", "Name", apartment.BuildingId);
            ViewBag.TypeApartmentList = GetTypeApartmentSelectList(apartment.TypeApartment);
            return View(apartment);
        }

        // GET: Apartments/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var apartment = await _context.Apartment.FindAsync(id);

            if (apartment == null)
            {
                return NotFound();
            }

            if (apartment.ImageData != null)
            {
                string imageBase64Data = Convert.ToBase64String(apartment.ImageData);
                string imageData = string.Format("data:image/jpg;base64,{0}", imageBase64Data);

                ViewBag.ImageData = imageData;

            }

            ViewBag.BuildingId = new SelectList(_context.Building, "Id", "Name", apartment.BuildingId);
            ViewBag.TypeApartmentList = GetTypeApartmentSelectList(apartment.TypeApartment);

            return View(apartment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Apartment apartment, IFormFile image)
        {
            if (id != apartment.Id)
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
                            apartment.ImageData = ms.ToArray();
                        }
                    }
                    else
                    {
                        var webRootPath = _hostingEnvironment.WebRootPath;
                        string defaultImagePath = Path.Combine(webRootPath, "img", "no_available_img.jpeg");

                        try
                        {
                            byte[] defaultImageData = System.IO.File.ReadAllBytes(defaultImagePath);
                            apartment.ImageData = defaultImageData;
                        }
                        catch (Exception ex)
                        {
                            return Content("Error to select image: " + ex.Message);
                        }
                    }


                    _context.Update(apartment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApartmentExists(apartment.Id))
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

            ViewBag.BuildingId = new SelectList(_context.Building, "Id", "Name", apartment.BuildingId);
            ViewBag.TypeApartmentList = GetTypeApartmentSelectList(apartment.TypeApartment);

            return View(apartment);
        }



        // GET: Apartments/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var apartment = await _context.Apartment
                .Include(a => a.Building)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (apartment == null)
            {
                return NotFound();
            }

            return View(apartment);
        }

        // POST: Apartments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var apartment = await _context.Apartment.FindAsync(id);
            if (apartment != null)
            {
                _context.Apartment.Remove(apartment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApartmentExists(Guid id)
        {
            return _context.Apartment.Any(e => e.Id == id);
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

    }
}
