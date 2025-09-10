// File: ViewModels/CourseViewModel.cs
using System.ComponentModel.DataAnnotations;

namespace ELearningPlatform.ViewModels
{
    public class ManageUserRolesViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public List<RoleViewModel> Roles { get; set; }
    }
}