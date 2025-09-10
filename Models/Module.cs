// File: Models/Module.cs
using System.ComponentModel.DataAnnotations;

namespace ELearningPlatform.Models
{
    public class Module
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        // Foreign key for the Course
        public int CourseId { get; set; }
        public Course Course { get; set; }

        // Navigation property
        public ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
    }
}