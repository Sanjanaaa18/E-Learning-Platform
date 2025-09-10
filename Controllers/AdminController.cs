// File: Controllers/AdminController.cs
using ELearningPlatform.Interfaces;
using ELearningPlatform.Models;
using ELearningPlatform.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ELearningPlatform.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(
            IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.TotalUsers = await _userManager.Users.CountAsync();
            ViewBag.TotalCourses = (await _unitOfWork.Courses.GetAllAsync()).Count();
            ViewBag.PendingCourses = (await _unitOfWork.Courses.FindAsync(c => !c.IsApproved)).Count();

            return View();
        }

        public async Task<IActionResult> ManageCourses()
        {
            var allCourses = await _unitOfWork.Courses.GetAllAsync();
            // This is not efficient for large datasets, but fine for this project.
            // In a real app, you would add a method to the repository to include instructors.
            return View(allCourses);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveCourse(int courseId)
        {
            var course = await _unitOfWork.Courses.GetByIdAsync(courseId);
            if (course != null)
            {
                course.IsApproved = true;
                _unitOfWork.Courses.Update(course);
                await _unitOfWork.CompleteAsync();
            }
            return RedirectToAction(nameof(ManageCourses));
        }

        public async Task<IActionResult> ManageUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(users);
        }

        // GET: Admin/ManageUserRoles/GUID
        public async Task<IActionResult> ManageUserRoles(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var viewModel = new ManageUserRolesViewModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                Roles = new List<RoleViewModel>()
            };

            foreach (var role in await _roleManager.Roles.ToListAsync())
            {
                var roleViewModel = new RoleViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                    IsSelected = await _userManager.IsInRoleAsync(user, role.Name)
                };
                viewModel.Roles.Add(roleViewModel);
            }
            return View(viewModel);
        }

        // POST: Admin/ManageUserRoles
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageUserRoles(ManageUserRolesViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null) return NotFound();

            var currentRoles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!result.Succeeded) return View(model); // Handle error

            result = await _userManager.AddToRolesAsync(user, model.Roles.Where(r => r.IsSelected).Select(r => r.RoleName));
            if (!result.Succeeded) return View(model); // Handle error

            return RedirectToAction(nameof(ManageUsers));
        }
    }
}