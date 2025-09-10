// File: Repositories/CourseRepository.cs
using ELearningPlatform.Data;
using ELearningPlatform.Interfaces;
using ELearningPlatform.Models;

namespace ELearningPlatform.Repositories
{
    public class LessonCompletionRepository : GenericRepository<LessonCompletion>, ILessonCompletionRepository
    {
        public LessonCompletionRepository(ApplicationDbContext context) : base(context)
        {
        }

        // Implement course-specific methods here later
    }
}