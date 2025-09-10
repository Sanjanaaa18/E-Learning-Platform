// File: Repositories/CourseRepository.cs
using ELearningPlatform.Data;
using ELearningPlatform.Interfaces;
using ELearningPlatform.Models;

namespace ELearningPlatform.Repositories
{
    public class EnrollmentRepository : GenericRepository<Enrollment>, IEnrollmentRepository
    {
        public EnrollmentRepository(ApplicationDbContext context) : base(context)
        {
        }

        // Implement course-specific methods here later
    }
}