using ELearningPlatform.Interfaces;
using ELearningPlatform.Models;
using ELearningPlatform.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ELearningPlatform.Controllers
{
    [Authorize(Roles = "Instructor")]
    public class CoursesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public CoursesController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        // GET: /Courses (Instructor's course list)
        public async Task<IActionResult> Index()
        {
            var currentUserId = _userManager.GetUserId(User);
            var courses = await _unitOfWork.Courses.FindAsync(c => c.InstructorId == currentUserId);
            return View(courses);
        }

        // GET: /Courses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Courses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
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
                    IsApproved = false
                };
                await _unitOfWork.Courses.AddAsync(course);
                await _unitOfWork.CompleteAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }

        // GET: /Courses/Details/5 (Manage Modules)
        //public async Task<IActionResult> Details(int id)
        //{
        //    var currentUserId = _userManager.GetUserId(User);
        //    var course = await _unitOfWork.Courses.GetCourseForInstructorAsync(id, currentUserId);
        //    if (course == null)
        //    {
        //        return NotFound();
        //    }
        //    var viewModel = new CourseDetailsViewModel { Course = course };
        //    return View(viewModel);
        //}

        public async Task<IActionResult> Details(int id)
        {
            var currentUserId = _userManager.GetUserId(User);
            var course = await _unitOfWork.Courses.GetCourseForInstructorAsync(id, currentUserId);
            if (course == null) return NotFound();

            // --- ADD THIS LOGIC TO GET COMPLETED STUDENTS ---
            var completedEnrollments = (await _unitOfWork.Enrollments
                .FindAsync(e => e.CourseId == id && e.IsCompleted))
                .ToList();

            var completedStudentEmails = new List<string>();
            foreach (var enrollment in completedEnrollments)
            {
                var student = await _userManager.FindByIdAsync(enrollment.StudentId);
                if (student != null)
                {
                    completedStudentEmails.Add(student.Email);
                }
            }
            // --- END OF NEW LOGIC ---

            var viewModel = new CourseDetailsViewModel
            {
                Course = course,
                CompletedStudentEmails = completedStudentEmails // Pass the list to the ViewModel
            };

            return View(viewModel);
        }


        // POST: /Courses/AddModule
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddModule(CourseDetailsViewModel viewModel)
        {
            var currentUserId = _userManager.GetUserId(User);
            var courseExists = (await _unitOfWork.Courses.FindAsync(c => c.Id == viewModel.Course.Id && c.InstructorId == currentUserId)).Any();

            if (!courseExists)
            {
                return Forbid();
            }

            if (!string.IsNullOrEmpty(viewModel.NewModuleTitle))
            {
                var newModule = new Module
                {
                    Title = viewModel.NewModuleTitle,
                    CourseId = viewModel.Course.Id
                };
                await _unitOfWork.Modules.AddAsync(newModule);
                await _unitOfWork.CompleteAsync();
                return RedirectToAction(nameof(Details), new { id = viewModel.Course.Id });
            }

            var course = await _unitOfWork.Courses.GetCourseForInstructorAsync(viewModel.Course.Id, currentUserId);
            viewModel.Course = course;
            ModelState.AddModelError("NewModuleTitle", "Module title cannot be empty.");
            return View("Details", viewModel);
        }

        // GET: Courses/Edit/5
        // This action displays the form with the course's current data.
        public async Task<IActionResult> Edit(int id)
        {
            var currentUserId = _userManager.GetUserId(User);
            var course = (await _unitOfWork.Courses.FindAsync(c => c.Id == id && c.InstructorId == currentUserId)).FirstOrDefault();

            if (course == null)
            {
                return NotFound();
            }

            var viewModel = new CourseViewModel
            {
                Id = course.Id,
                Title = course.Title,
                Description = course.Description
            };

            return View(viewModel);
        }

        // POST: Courses/Edit/5
        // This action saves the updated course data to the database.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CourseViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var currentUserId = _userManager.GetUserId(User);
                var courseToUpdate = (await _unitOfWork.Courses.FindAsync(c => c.Id == id && c.InstructorId == currentUserId)).FirstOrDefault();

                if (courseToUpdate == null)
                {
                    return NotFound();
                }

                courseToUpdate.Title = viewModel.Title;
                courseToUpdate.Description = viewModel.Description;

                _unitOfWork.Courses.Update(courseToUpdate);
                await _unitOfWork.CompleteAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }


        [Authorize(Roles = "Instructor")]

        public async Task<IActionResult> Delete(int id)

        {

            var currentUserId = _userManager.GetUserId(User);

            var course = (await _unitOfWork.Courses.FindAsync(c => c.Id == id && c.InstructorId == currentUserId)).FirstOrDefault();



            if (course == null)

            {

                return NotFound();

            }



            return View(course); // Confirmation page

        }



        [HttpPost, ActionName("Delete")]

        [ValidateAntiForgeryToken]

        [Authorize(Roles = "Instructor")]

        public async Task<IActionResult> DeleteConfirmed(int id)

        {

            var currentUserId = _userManager.GetUserId(User);

            var course = (await _unitOfWork.Courses.FindAsync(c => c.Id == id && c.InstructorId == currentUserId)).FirstOrDefault();



            if (course == null)

            {

                return NotFound();

            }



            _unitOfWork.Courses.Delete(course);



            await _unitOfWork.CompleteAsync();



            return RedirectToAction(nameof(Index));

        }

    }
}