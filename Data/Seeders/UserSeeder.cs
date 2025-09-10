// File: Data/Seeders/UserSeeder.cs
using ELearningPlatform.Models;
using Microsoft.AspNetCore.Identity;

namespace ELearningPlatform.Data.Seeders
{
    public static class UserSeeder
    {
        public static async Task SeedAdminUserAsync(IServiceProvider serviceProvider)
        {
            // Get the UserManager and RoleManager services

            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Define admin user details

            string adminEmail = "admin@elearning.com";
            string adminPassword = "AdminPassword123!"; // Use a strong password in production

            // Check if the admin role exists

            if (await roleManager.RoleExistsAsync("Admin"))
            {
                // Check if the admin user already exists
                if (await userManager.FindByEmailAsync(adminEmail) == null)
                {
                    ApplicationUser adminUser = new ApplicationUser
                    {
                        UserName = adminEmail,
                        Email = adminEmail,
                        EmailConfirmed = true // Or implement an email confirmation flow
                    };

                    // Create the admin user
                    IdentityResult result = await userManager.CreateAsync(adminUser, adminPassword);

                    // If user created successfully, assign to Admin role
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(adminUser, "Admin");
                    }

                }

            }

        }

    }

}