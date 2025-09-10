// File: Data/Seeders/RoleSeeder.cs
using Microsoft.AspNetCore.Identity;

namespace ELearningPlatform.Data.Seeders
{
    public static class RoleSeeder
    {
        public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
        {
            // Get the RoleManager service
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Define the roles
            string[] roleNames = { "Admin", "Instructor", "Student" };

            foreach (var roleName in roleNames)
            {
                // Check if the role already exists
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    // Create the role if it doesn't exist
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }
    }
}