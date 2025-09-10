using Microsoft.AspNetCore.Identity;
namespace ELearningPlatform.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Additional properties can be added here in the future
        public string? FullName { get; set; }
    }
}
