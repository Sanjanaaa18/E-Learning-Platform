// File: Interfaces/ICourseRepository.cs
using ELearningPlatform.Models;

namespace ELearningPlatform.Interfaces
{
    public interface ICourseRepository : IGenericRepository<Course>
    {
        Task<IEnumerable<Course>> GetApprovedCoursesWithInstructorsAsync();
        Task<Course> GetCourseForInstructorAsync(int courseId, string instructorId);
        Task<Course> GetCourseDetailsAsync(int id);
    }
}