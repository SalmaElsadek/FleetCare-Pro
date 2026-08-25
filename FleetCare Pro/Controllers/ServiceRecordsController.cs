using AutoMapper;
using FleetCare_Pro.Data;
using FleetCare_Pro.Models;
using FleetCare_Pro.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FleetCare_Pro.Controllers
{
    [Authorize]
    public class ServiceRecordsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ServiceRecordsController(ApplicationDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        //GET: ServiceRecords/Index
        public async Task<IActionResult> Index()
        {
            var records = await _context.ServiceRecords
                .Include(s => s.Vehicle)
                .Include(s => s.ServiceCenter)
                .Include(s => s.ServiceLineItems)
                .ThenInclude(li => li.ServiceCategory)
                .ToListAsync();

            return View(records);
        }

        //GET: ServiceRecords/Create 
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Vehicles = new SelectList(await _context.Vehicles.ToListAsync(), "Id", "LicensePlate");
            ViewBag.ServiceCenters = new SelectList(await _context.ServiceCenters.ToListAsync(), "Id", "Name");
            ViewBag.ServiceCategories = await _context.ServiceCategories.ToListAsync();

            var vm = new ServiceFormViewModel
            {
                ServiceDate = DateTime.Today,
                ServiceLineItems = new List<ServiceItemFormViewModel> { new ServiceItemFormViewModel() }
            };

            return View(vm); 
        }

        //POST: ServiceRecords/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceFormViewModel vm)
        {
            if (ModelState.IsValid)
            {
                // (Atomic Operation)
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    var serviceRecord = _mapper.Map<ServiceRecord>(vm);
                    serviceRecord.TotalCost = vm.ServiceLineItems.Sum(item => item.Cost);
                    serviceRecord.CreatedByUserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "System";
                    serviceRecord.ServiceLineItems = _mapper.Map<ICollection<ServiceLineItem>>(vm.ServiceLineItems);

                    if (vm.InvoiceFile != null && vm.InvoiceFile.Length > 0)
                    {
                        //(أقصى حاجة 5 ميجابايت = 5 * 1024 * 1024 بايت)
                        if (vm.InvoiceFile.Length > 5 * 1024 * 1024)
                        {
                            ModelState.AddModelError("InvoiceFile", "File size must not exceed 5MB.");
                            throw new Exception("File size exceeds limit.");
                        }

                        //(.pdf, .jpg, .jpeg, .png)
                        var allowedExtensions = new[] { ".pdf", ".jpg", ".jpeg", ".png" };
                        var extension = Path.GetExtension(vm.InvoiceFile.FileName).ToLowerInvariant();

                        if (!allowedExtensions.Contains(extension))
                        {
                            ModelState.AddModelError("InvoiceFile", "Only .pdf, .jpg, .jpeg, and .png files are allowed.");
                            throw new Exception("Invalid file extension.");
                        }

                        //wwwroot/uploads/invoices
                        string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "invoices");
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(vm.InvoiceFile.FileName);
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await vm.InvoiceFile.CopyToAsync(fileStream);
                        }

                        serviceRecord.InvoiceDocumentPath = "/uploads/invoices/" + uniqueFileName;
                    }

                    _context.ServiceRecords.Add(serviceRecord);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    TempData["SuccessMessage"] = "Service record has been successfully added!";
                    return Redirect(nameof(Index));
                }
                catch (Exception)
                {
                    // لو حصل أي خطأ، نراجع عن كل حاجة اتعملت
                    await transaction.RollbackAsync();
                    ModelState.AddModelError("", "An error occurred while saving the service record. Please try again.");
                }
            }

            if (vm.ServiceLineItems == null || !vm.ServiceLineItems.Any())
            {
                vm.ServiceLineItems = new List<ServiceItemFormViewModel> { new ServiceItemFormViewModel() };
            }

            ViewBag.Vehicles = new SelectList(await _context.Vehicles.ToListAsync(), "Id", "LicensePlate", vm.VehicleId);
            ViewBag.ServiceCenters = new SelectList(await _context.ServiceCenters.ToListAsync(), "Id", "Name", vm.ServiceCenterId);
            ViewBag.ServiceCategories = await _context.ServiceCategories.ToListAsync();

            return View(vm);
        }

        //GET: ServiceRecords/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceRecord = await _context.ServiceRecords
                .Include(s => s.Vehicle)
                .Include(s => s.ServiceCenter)
                .Include(s => s.ServiceLineItems)
                .ThenInclude(li => li.ServiceCategory)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (serviceRecord == null)
            {
                return NotFound();
            }

            return View(serviceRecord);
        }
    }
}