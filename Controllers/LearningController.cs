// File: Controllers/LearningController.cs
using ELearningPlatform.Interfaces;
using ELearningPlatform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ELearningPlatform.Controllers
{
    [Authorize(Roles = "Student")]
    public class LearningController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public LearningController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        // Action methods will go here
        public async Task<IActionResult> Index()
        {
            var studentId = _userManager.GetUserId(User);
            var enrollments = await _unitOfWork.Enrollments.FindAsync(e => e.StudentId == studentId);

            // We need to load the course for each enrollment
            foreach (var enrollment in enrollments)
            {
                enrollment.Course = await _unitOfWork.Courses.GetByIdAsync(enrollment.CourseId);
            }

            return View(enrollments);
        }

        //public async Task<IActionResult> Course(int courseId)
        //{
        //    var studentId = _userManager.GetUserId(User);

        //    // Security check: ensure student is enrolled in this course
        //    var enrollment = (await _unitOfWork.Enrollments.FindAsync(e => e.StudentId == studentId && e.CourseId == courseId)).FirstOrDefault();
        //    if (enrollment == null)
        //    {
        //        return Forbid(); // Or redirect with an error message
        //    }

        //    var course = await _unitOfWork.Courses.GetCourseDetailsAsync(courseId);
        //    if (course == null)
        //    {
        //        return NotFound();
        //    }

        //    // Get completed lesson IDs for this student in this course
        //    var completedLessons = await _unitOfWork.LessonCompletions
        //        .FindAsync(lc => lc.StudentId == studentId && course.Modules.SelectMany(m => m.Lessons).Select(l => l.Id).Contains(lc.LessonId));

        //    ViewData["CompletedLessonIds"] = completedLessons.Select(lc => lc.LessonId).ToList();

        //    return View(course);
        //}

        //public async Task<IActionResult> Course(int courseId)
        //{
        //    var studentId = _userManager.GetUserId(User);

        //    // Security check: ensure student is enrolled in this course
        //    var enrollment = (await _unitOfWork.Enrollments
        //        .FindAsync(e => e.StudentId == studentId && e.CourseId == courseId))
        //        .FirstOrDefault();

        //    if (enrollment == null)
        //    {
        //        return Forbid(); // Or redirect with an error message
        //    }

        //    var course = await _unitOfWork.Courses.GetCourseDetailsAsync(courseId);
        //    if (course == null)
        //    {
        //        return NotFound();
        //    }

        //    // ✅ Load lesson IDs into memory first
        //    var lessonIds = course.Modules
        //        .SelectMany(m => m.Lessons)
        //        .Select(l => l.Id)
        //        .ToList();

        //    // ✅ Now query LessonCompletions with lessonIds
        //    var completedLessons = await _unitOfWork.LessonCompletions
        //        .FindAsync(lc => lc.StudentId == studentId && lessonIds.Contains(lc.LessonId));

        //    ViewData["CompletedLessonIds"] = completedLessons
        //        .Select(lc => lc.LessonId)
        //        .ToList();

        //    return View(course);
        //}


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> MarkAsComplete(int lessonId, int courseId)
        //{
        //    var studentId = _userManager.GetUserId(User);

        //    // Security check: ensure student is enrolled
        //    var enrollment = (await _unitOfWork.Enrollments.FindAsync(e => e.StudentId == studentId && e.CourseId == courseId)).FirstOrDefault();
        //    if (enrollment == null)
        //    {
        //        return Forbid();
        //    }

        //    // Check if already completed
        //    var alreadyCompleted = (await _unitOfWork.LessonCompletions.FindAsync(lc => lc.LessonId == lessonId && lc.StudentId == studentId)).Any();
        //    if (!alreadyCompleted)
        //    {
        //        var completion = new LessonCompletion
        //        {
        //            StudentId = studentId,
        //            LessonId = lessonId,
        //            CompletionDate = DateTime.UtcNow
        //        };
        //        await _unitOfWork.LessonCompletions.AddAsync(completion);
        //        await _unitOfWork.CompleteAsync();
        //    }

        //    return RedirectToAction("Course", new { courseId = courseId });
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> MarkAsComplete(int lessonId, int courseId)
        //{
        //    var studentId = _userManager.GetUserId(User);

        //    // Security check: ensure student is enrolled
        //    var enrollment = (await _unitOfWork.Enrollments
        //        .FindAsync(e => e.StudentId == studentId && e.CourseId == courseId))
        //        .FirstOrDefault();

        //    if (enrollment == null)
        //        return Json(new { success = false, message = "Not enrolled in this course." });

        //    // Check if already completed
        //    var alreadyCompleted = (await _unitOfWork.LessonCompletions
        //        .FindAsync(lc => lc.LessonId == lessonId && lc.StudentId == studentId))
        //        .Any();

        //    if (!alreadyCompleted)
        //    {
        //        var completion = new LessonCompletion
        //        {
        //            StudentId = studentId,
        //            LessonId = lessonId,
        //            CompletionDate = DateTime.UtcNow
        //        };
        //        await _unitOfWork.LessonCompletions.AddAsync(completion);
        //        await _unitOfWork.CompleteAsync();
        //    }

        //    return Json(new { success = true, lessonId = lessonId });
        //}

        // Add this new action to handle the button click
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkCourseAsComplete(int courseId)
        {
            var studentId = _userManager.GetUserId(User);
            var enrollment = (await _unitOfWork.Enrollments
                .FindAsync(e => e.StudentId == studentId && e.CourseId == courseId))
                .FirstOrDefault();

            if (enrollment != null)
            {
                enrollment.IsCompleted = true;
                enrollment.CompletionDate = DateTime.UtcNow;
                _unitOfWork.Enrollments.Update(enrollment);
                await _unitOfWork.CompleteAsync();
            }

            return RedirectToAction("Course", new { courseId = courseId });
        }

        // Update the existing "Course" action to pass the completion status to the view
        public async Task<IActionResult> Course(int courseId)
        {
            var studentId = _userManager.GetUserId(User);
            var enrollment = (await _unitOfWork.Enrollments
                .FindAsync(e => e.StudentId == studentId && e.CourseId == courseId))
                .FirstOrDefault();

            if (enrollment == null) return Forbid(); // Security check

            // Pass the enrollment status to the view
            ViewData["IsCourseCompleted"] = enrollment.IsCompleted;

            var course = await _unitOfWork.Courses.GetCourseDetailsAsync(courseId);
            if (course == null) return NotFound();

            var completedLessons = await _unitOfWork.LessonCompletions.FindAsync(lc => lc.StudentId == studentId);
            ViewData["CompletedLessonIds"] = completedLessons.Select(lc => lc.LessonId).ToList();

            return View(course);
        }

    }
}