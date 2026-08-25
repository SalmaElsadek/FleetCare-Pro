using FleetCare_Pro.Models;
using Microsoft.AspNetCore.Identity;

namespace FleetCare_Pro.Data.Seeder
{
    public class RoleSeeder
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<Authentication>>();

            string[] roleNames = { "Admin", "FleetManager", "Driver" };

            foreach (var it in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(it);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(it));
                }
            }

            var adminEmail = "admin@gmail.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                var newAdmin = new Authentication
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FullName = "System Administrator",
                    EmployeeId = "ADM-001"
                };

                var createPowerUser = await userManager.CreateAsync(newAdmin, "Admin@0123");
                if (createPowerUser.Succeeded)
                {
                    //connect to the role
                    await userManager.AddToRoleAsync(newAdmin, "Admin");
                }
            }
        }
    }
}
