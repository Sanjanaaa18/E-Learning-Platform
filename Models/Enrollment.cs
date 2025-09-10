// File: Models/Enrollment.cs
using System.ComponentModel.DataAnnotations;

namespace ELearningPlatform.Models
{
    public class Enrollment
    {
        public int Id { get; set; }

        // Foreign key for the Student
        public string StudentId { get; set; }
        public ApplicationUser Student { get; set; }

        // Foreign key for the Course
        public int CourseId { get; set; }
        public Course Course { get; set; }

        [DataType(DataType.Date)]
        public DateTime EnrollmentDate { get; set; }


    }
}