using ELearningPlatform.Models;
using System.ComponentModel.DataAnnotations;

namespace ELearningPlatform.ViewModels
{
    public class CourseDetailsViewModel
    {
        public Course Course { get; set; }

        [Required(ErrorMessage = "Module title cannot be empty.")]
        [StringLength(100)]
        [Display(Name = "New Module Title")]
        public string NewModuleTitle { get; set; }
    }
}