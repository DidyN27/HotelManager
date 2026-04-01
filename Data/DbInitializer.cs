using Microsoft.AspNetCore.Identity;
using Hotel.Models;


public static class DbInitializer
{
    public static async Task SeedAdmin(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
        }

        if (await userManager.FindByEmailAsync("admin@hotel.com") == null)
        {
            var admin = new ApplicationUser
            {
                UserName = "admin",
                Email = "admin@hotel.com",
                FirstName = "System",
                MiddleName = "Hotel",
                LastName = "Admin",
                EGN = "0000000000",
                PhoneNumber = "0888000000",
                AppointmentDate = DateTime.Now,
                IsActive = true
            };

            await userManager.CreateAsync(admin, "Admin123!");
            await userManager.AddToRoleAsync(admin, "Admin");
        }
    }
}