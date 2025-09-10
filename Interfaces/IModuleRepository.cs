// File: Interfaces/ICourseRepository.cs
using ELearningPlatform.Models;

namespace ELearningPlatform.Interfaces
{
    public interface IModuleRepository : IGenericRepository<Module>
    {
        // Add course-specific methods here later if needed
        // For example: Task<IEnumerable<Course>> GetCoursesWithModulesAsync();
    }
}