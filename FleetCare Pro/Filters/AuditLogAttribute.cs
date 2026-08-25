using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using FleetCare_Pro.Data;
using FleetCare_Pro.Models;

namespace FleetCare_Pro.Filters
{
    public class AuditLogAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // تنفيذ الأكشن الأول والتأكد إنه تم بنجاح
            var executedContext = await next();

            string method = context.HttpContext.Request.Method;
            if (method.Equals("POST", StringComparison.OrdinalIgnoreCase) ||
                method.Equals("PUT", StringComparison.OrdinalIgnoreCase) ||
                method.Equals("DELETE", StringComparison.OrdinalIgnoreCase))
            {
                var dbContext = context.HttpContext.RequestServices.GetRequiredService<ApplicationDbContext>();

                string userId = context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "Anonymous";
                string controllerName = context.RouteData.Values["controller"]?.ToString() ?? "Unknown";
                string actionName = context.RouteData.Values["action"]?.ToString() ?? "Unknown";

                var auditLog = new AuditLog
                {
                    UserId = userId,
                    Timestamp = DateTime.UtcNow,
                    ActionDetails = $"Executed {method} on {controllerName}/{actionName}"
                };

                dbContext.AuditLogs.Add(auditLog);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}