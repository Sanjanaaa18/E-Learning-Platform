// File: Models/Course.cs
using ELearningPlatform.Models;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ELearningPlatform.Models
{
    public class Course
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        // Foreign key for the Instructor (who is a user)
        public string InstructorId { get; set; }
        public ApplicationUser Instructor { get; set; }

        public bool IsApproved { get; set; } = false; // Admin must approve courses [cite: 20]

        // Navigation properties
        public ICollection<Module> Modules { get; set; } = new List<Module>();
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}