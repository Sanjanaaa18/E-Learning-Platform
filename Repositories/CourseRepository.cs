// File: Repositories/CourseRepository.cs
using ELearningPlatform.Data;
using ELearningPlatform.Interfaces;
using ELearningPlatform.Models;
using Microsoft.EntityFrameworkCore;

namespace ELearningPlatform.Repositories
{
    public class CourseRepository : GenericRepository<Course>, ICourseRepository
    {
        public CourseRepository(ApplicationDbContext context) : base(context)
        {
        }

        // Implement course-specific methods here later
        public async Task<IEnumerable<Course>> GetApprovedCoursesWithInstructorsAsync()
        {
            return await _context.Courses
                .Include(c => c.Instructor) // Eager-load the Instructor
                .Where(c => c.IsApproved)
                .ToListAsync();
        }

        public async Task<Course> GetCourseDetailsAsync(int id)
        {
            return await _context.Courses
                .Include(c => c.Instructor)
                .Include(c => c.Modules)
                    .ThenInclude(m => m.Lessons) // Eager-load Modules and their Lessons
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}