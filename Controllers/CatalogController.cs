// File: Controllers/CatalogController.cs
using ELearningPlatform.Interfaces;
using ELearningPlatform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ELearningPlatform.Controllers
{
    public class CatalogController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public CatalogController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        // GET: Catalog
        [AllowAnonymous] // Everyone can view the catalog
        public async Task<IActionResult> Index()
        {
            var courses = await _unitOfWork.Courses.GetApprovedCoursesWithInstructorsAsync();
            return View(courses);
        }

        // GET: Catalog/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var course = await _unitOfWork.Courses.GetCourseDetailsAsync(id);
            if (course == null || !course.IsApproved)
            {
                return NotFound();
            }

            // Check if the current user (if logged in) is already enrolled
            ViewData["IsEnrolled"] = false;
            if (User.Identity.IsAuthenticated)
            {
                var currentUserId = _userManager.GetUserId(User);
                var enrollment = await _unitOfWork.Enrollments.FindAsync(e => e.CourseId == id && e.StudentId == currentUserId);
                if (enrollment.Any())
                {
                    ViewData["IsEnrolled"] = true;
                }
            }

            return View(course);
        }

        // POST: Catalog/Enroll
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Student")] // Only Students can enroll
        public async Task<IActionResult> Enroll(int courseId)
        {
            var course = await _unitOfWork.Courses.GetByIdAsync(courseId);
            var currentUserId = _userManager.GetUserId(User);

            if (course == null || !course.IsApproved)
            {
                return NotFound(); // Course doesn't exist or is not approved
            }

            // Prevent duplicate enrollment
            var isAlreadyEnrolled = (await _unitOfWork.Enrollments
                .FindAsync(e => e.CourseId == courseId && e.StudentId == currentUserId))
                .Any();

            if (isAlreadyEnrolled)
            {
                // Optionally, add a message to TempData to inform the user
                return RedirectToAction(nameof(Details), new { id = courseId });
            }

            var enrollment = new Enrollment
            {
                CourseId = courseId,
                StudentId = currentUserId,
                EnrollmentDate = DateTime.UtcNow
            };

            await _unitOfWork.Enrollments.AddAsync(enrollment);
            await _unitOfWork.CompleteAsync();

            return RedirectToAction(nameof(Details), new { id = courseId });
        }
    }
}