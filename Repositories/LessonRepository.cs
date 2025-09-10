// File: Repositories/CourseRepository.cs
using ELearningPlatform.Data;
using ELearningPlatform.Interfaces;
using ELearningPlatform.Models;

namespace ELearningPlatform.Repositories
{
    public class LessonRepository : GenericRepository<Lesson>, ILessonRepository
    {
        public LessonRepository(ApplicationDbContext context) : base(context)
        {
        }

        // Implement course-specific methods here later
    }
}