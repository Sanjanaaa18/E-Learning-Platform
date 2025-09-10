// File: Interfaces/ICourseRepository.cs
using ELearningPlatform.Models;

namespace ELearningPlatform.Interfaces
{
    public interface ILessonRepository : IGenericRepository<Lesson>
    {
        // Add course-specific methods here later if needed
        // For example: Task<IEnumerable<Course>> GetCoursesWithModulesAsync();
    }
}