// File: Models/Lesson.cs
using System.ComponentModel.DataAnnotations;

namespace ELearningPlatform.Models
{
    public class Lesson
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        // Content could be text, markdown, or a URL to a video/PDF [cite: 22]
        public string Content { get; set; }

        // Foreign key for the Module
        public int ModuleId { get; set; }
        public Module Module { get; set; }
    }
}