// File: Models/LessonCompletion.cs
using System.ComponentModel.DataAnnotations;

namespace ELearningPlatform.Models
{
    public class LessonCompletion
    {
        public int Id { get; set; }

        [Required]
        public string StudentId { get; set; }
        public ApplicationUser Student { get; set; }

        [Required]
        public int LessonId { get; set; }
        public Lesson Lesson { get; set; }

        public DateTime CompletionDate { get; set; }
    }
}