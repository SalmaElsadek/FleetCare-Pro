using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace FleetCare_Pro.Middlewares
{
    public class MaintenanceModeMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public MaintenanceModeMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            bool isMaintenanceMode = _configuration.GetValue<bool>("IsMaintenanceMode");

            if (isMaintenanceMode)
            {
                var path = context.Request.Path;
                if (!path.StartsWithSegments("/Home/Maintenance") && !path.StartsWithSegments("/lib") && !path.StartsWithSegments("/css"))
                {
                    context.Response.Redirect("/Home/Maintenance");
                    return;
                }
            }

            await _next(context);
        }
    }
}