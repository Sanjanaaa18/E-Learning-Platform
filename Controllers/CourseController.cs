// File: Controllers/CoursesController.cs
using ELearningPlatform.Interfaces;
using ELearningPlatform.Models;
using ELearningPlatform.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ELearningPlatform.Controllers
{
    [Authorize] // Ensures only logged-in users can access this controller
    public class CoursesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public CoursesController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        // We will add action methods here
        [Authorize(Roles = "Instructor")] // Restrict access to only Instructors
        public async Task<IActionResult> Index()
        {
            var currentUserId = _userManager.GetUserId(User);
            var courses = await _unitOfWork.Courses.FindAsync(c => c.InstructorId == currentUserId);
            return View(courses);
        }

        // GET: Courses/Create
        [Authorize(Roles = "Instructor")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Courses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> Create(CourseViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var currentUserId = _userManager.GetUserId(User);

                var course = new Course
                {
                    Title = viewModel.Title,
                    Description = viewModel.Description,
                    InstructorId = currentUserId,
                    IsApproved = false // Courses are not approved by default
                };

                await _unitOfWork.Courses.AddAsync(course);
                await _unitOfWork.CompleteAsync(); // Save changes to the database

                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }
        // GET: Courses/Details/5
        // This action displays the course details and the form to add modules.
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> Details(int id)
        {
            var currentUserId = _userManager.GetUserId(User);

            // Fetch the course and include its existing modules
            var course = await _unitOfWork.Courses.FindAsync(c => c.Id == id && c.InstructorId == currentUserId);

            // Eager load modules
            course.First().Modules = (ICollection<Module>)await _unitOfWork.Modules.FindAsync(m => m.CourseId == id);

            if (course.FirstOrDefault() == null)
            {
                // This prevents an instructor from accessing another instructor's course
                return NotFound();
            }

            var viewModel = new CourseDetailsViewModel
            {
                Course = course.FirstOrDefault()
            };

            return View(viewModel);
        }

        // POST: Courses/AddModule
        // This action processes the form submission to add a new module.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> AddModule(CourseDetailsViewModel viewModel)
        {
            var currentUserId = _userManager.GetUserId(User);
            var course = (await _unitOfWork.Courses.FindAsync(c => c.Id == viewModel.Course.Id && c.InstructorId == currentUserId)).FirstOrDefault();

            if (course == null)
            {
                // Security check failed
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                var newModule = new Module
                {
                    Title = viewModel.NewModuleTitle,
                    CourseId = course.Id
                };

                await _unitOfWork.Modules.AddAsync(newModule);
                await _unitOfWork.CompleteAsync();

                // Redirect back to the same details page to see the new module
                return RedirectToAction(nameof(Details), new { id = course.Id });
            }

            // If model state is invalid, reload the page with the course data
            viewModel.Course = course;
            return View("Details", viewModel);
        }

    }
}