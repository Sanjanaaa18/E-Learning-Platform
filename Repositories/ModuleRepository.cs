// File: Repositories/CourseRepository.cs
using ELearningPlatform.Data;
using ELearningPlatform.Interfaces;
using ELearningPlatform.Models;

namespace ELearningPlatform.Repositories
{
    public class ModuleRepository : GenericRepository<Module>, IModuleRepository
    {
        public ModuleRepository(ApplicationDbContext context) : base(context)
        {
        }

        // Implement course-specific methods here later
    }
}