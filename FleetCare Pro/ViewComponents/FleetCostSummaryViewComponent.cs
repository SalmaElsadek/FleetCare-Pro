using FleetCare_Pro.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FleetCare_Pro.ViewComponents
{
    public class FleetCostSummaryViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public FleetCostSummaryViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            decimal monthlySpend = await _context.ServiceRecords
                .Where(sr => sr.ServiceDate >= startDate && sr.ServiceDate <= endDate)
                .SumAsync(sr => (decimal?)sr.TotalCost) ?? 0;

            return View(monthlySpend);
        }
    }
}