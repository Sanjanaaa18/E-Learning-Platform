using ELearningPlatform.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ELearningPlatform.ViewModels
{
    public class CourseDetailsViewModel
    {
        // The course being displayed
        public Course Course { get; set; }

        // New module title for adding a module
        [Required(ErrorMessage = "Module title cannot be empty.")]
        [StringLength(100, ErrorMessage = "Module title cannot exceed 100 characters.")]
        [Display(Name = "New Module Title")]
        public string NewModuleTitle { get; set; }



        // Optional: For listing modules separately if needed
        public List<Module> Modules { get; set; } = new List<Module>();
    }
}
