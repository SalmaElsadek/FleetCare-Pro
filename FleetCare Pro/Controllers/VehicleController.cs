using AutoMapper;
using FleetCare_Pro.Data;
using FleetCare_Pro.Filters;
using FleetCare_Pro.Models;
using FleetCare_Pro.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FleetCare_Pro.Controllers
{
    [Authorize(Roles = "Admin,FleetManager")]
    public class VehicleController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment; // to reach wwwroot
        private readonly UserManager<Authentication> _userManager;
        private readonly IMapper _mapper;

        public VehicleController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, UserManager<Authentication> userManager, IMapper mapper)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
            _mapper = mapper;
        }

        // GET: Vehicle
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var vehicles = await _context.Vehicles.Include(v => v.Driver).ToListAsync();
            return View(vehicles);
        }

        // GET: Vehicle/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            // Dropdown list for people with driver role
            var drivers = await _userManager.GetUsersInRoleAsync("Driver");
            //temp to put data from controller to send and open atthe view
            ViewBag.Drivers = new SelectList(drivers, "Id", "FullName");

            return View(new VehicleFormViewModel());
        }

        //POST: Vehicle/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuditLog]
        public async Task<IActionResult> Create(VehicleFormViewModel vm)
        {
            if (ModelState.IsValid)
            {
                string? uniqueFileName = null;

                if (vm.VehicleImage != null)
                {
                    //  wwwroot/uploads/vehicles
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "vehicles");

                    // Creates Uplodes folder is it is not exist
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    uniqueFileName = Guid.NewGuid().ToString() + "_" + vm.VehicleImage.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await vm.VehicleImage.CopyToAsync(fileStream);
                    }
                }
                Vehicle newVehicle = _mapper.Map<Vehicle>(vm);
                newVehicle.VehicleImageURL = uniqueFileName ?? "";

                _context.Add(newVehicle);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Vehicle has been successfully added!";
                return RedirectToAction(nameof(Index));
            }

            var drivers = await _userManager.GetUsersInRoleAsync("Driver");
            ViewBag.Drivers = new SelectList(drivers, "Id", "FullName");
            return View(vm);
        }

        //GET: Vehicle/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null) return NotFound();

            var vm = _mapper.Map<VehicleFormViewModel>(vehicle);

            //الصورة القديمة عشان لو المستخدم مرفعش صورة جديدة
            vm.ExistingImageURL = vehicle.VehicleImageURL;

            var drivers = await _userManager.GetUsersInRoleAsync("Driver");
            ViewBag.Drivers = new SelectList(drivers, "Id", "FullName", vehicle.DriverId);

            return View(vm);
        }

        //POST: Vehicle/Edit/5 
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuditLog]
        public async Task<IActionResult> Edit(int id, VehicleFormViewModel vm)
        {
            if (id != vm.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var vehicle = await _context.Vehicles.FindAsync(id);
                if (vehicle == null) return NotFound();

                if (vm.VehicleImage != null)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "vehicles");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + vm.VehicleImage.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await vm.VehicleImage.CopyToAsync(fileStream);
                    }
                    vehicle.VehicleImageURL = uniqueFileName;
                }
                // لو مرفعش صورة هيفضل محتفظ بقيمته القديمة اللي في الداتا بيز

                //update
                _mapper.Map(vm, vehicle);

                _context.Update(vehicle);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Vehicle has been successfully updated!";
                return RedirectToAction(nameof(Index));
            }

            var drivers = await _userManager.GetUsersInRoleAsync("Driver");
            ViewBag.Drivers = new SelectList(drivers, "Id", "FullName", vm.DriverId);
            return View(vm);
        }

        //GET: Vehicles/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var vehicle = await _context.Vehicles
                .Include(v => v.Driver)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (vehicle == null) return NotFound();

            return View(vehicle); 
        }

        //GET: Vehicles/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var vehicle = await _context.Vehicles
                .Include(v => v.Driver)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (vehicle == null) return NotFound();

            return View(vehicle);
        }

        // POST: Vehicles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [AuditLog]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle != null)
            {
                // مسح الصورة من فولدر السيرفر لتوفير المساحة
                if (!string.IsNullOrEmpty(vehicle.VehicleImageURL))
                {
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "vehicles", vehicle.VehicleImageURL);
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                _context.Vehicles.Remove(vehicle);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Vehicle and its image have been successfully deleted!";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
