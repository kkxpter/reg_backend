using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RegSystemAPI.Models
{
    [Table("COURSES")]
    public class Course
    {
        [Key]
        [Column("COURSE_CODE")]
        public string CourseCode { get; set; } = string.Empty;

        [Column("COURSE_NAME")]
        public string CourseName { get; set; } = string.Empty;

        [Column("CREDITS")]
        public int Credits { get; set; }

        [Column("CATEGORY")]
        public string Category { get; set; } = string.Empty;

        [Column("PREREQUISITE_CODE")]
        public string? PrerequisiteCode { get; set; }
    }
}