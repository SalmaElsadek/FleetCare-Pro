using FleetCare_Pro.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FleetCare_Pro.ViewComponents
{
    public class OverdueMaintenanceViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public OverdueMaintenanceViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var sixMonthsAgo = DateTime.Now.AddMonths(-6);
            var overdueVehicles = await _context.Vehicles
                .Where(v => !v.ServiceRecords.Any(sr => sr.ServiceDate >= sixMonthsAgo))
                .ToListAsync();

            return View(overdueVehicles);
        }
    }
}