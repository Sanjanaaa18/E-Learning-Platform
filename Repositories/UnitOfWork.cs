// File: Repositories/UnitOfWork.cs
using ELearningPlatform.Data;
using ELearningPlatform.Interfaces;

namespace ELearningPlatform.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public ICourseRepository Courses { get; private set; }
        public IModuleRepository Modules { get; private set; }
        public ILessonRepository Lessons { get; private set; }
        public IEnrollmentRepository Enrollments { get; private set; }
        public ILessonCompletionRepository LessonCompletions { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Courses = new CourseRepository(_context);
            Modules = new ModuleRepository(_context);
            Lessons = new LessonRepository(_context);
            Enrollments = new EnrollmentRepository(_context);
            LessonCompletions = new LessonCompletionRepository(_context);
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}