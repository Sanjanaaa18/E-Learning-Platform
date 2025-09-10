// File: Interfaces/IUnitOfWork.cs
namespace ELearningPlatform.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ICourseRepository Courses { get; }
        IModuleRepository Modules { get; }
        ILessonRepository Lessons { get; }
        IEnrollmentRepository Enrollments { get; }
        ILessonCompletionRepository LessonCompletions { get; }

        Task<int> CompleteAsync();
    }
}