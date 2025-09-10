using System.ComponentModel.DataAnnotations;

namespace ELearningPlatform.ViewModels
{
    public class RegisterViewModel
    {
        [Required, EmailAddress]
        public string Email { get; set; } = default!;

        [Required]
        public string FullName { get; set; } = default!;

        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = default!;

        [Required]
        public string Role { get; set; } = "Student";
    }
}