using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RegSystemAPI.Models
{
    [Table("SECTIONS")]
    public class Section
    {
        [Key]
        [Column("SECTION_ID")]
        public int SectionId { get; set; }

        [Column("COURSE_CODE")]
        public string CourseCode { get; set; } = string.Empty;

        [Column("SEC_NO")]
        public string SecNo { get; set; } = string.Empty;

        [Column("SCHEDULE_TIME")]
        public string ScheduleTime { get; set; } = string.Empty;

        [Column("INSTRUCTOR")]
        public string Instructor { get; set; } = string.Empty;

        [Column("CAPACITY")]
        public int Capacity { get; set; }

        [Column("ENROLLED")]
        public int Enrolled { get; set; }

        // Foreign Key เชื่อมไปยัง Course
        [ForeignKey("CourseCode")]
        public Course? Course { get; set; }
    }
}