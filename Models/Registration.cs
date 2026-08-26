using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RegSystemAPI.Models
{
    [Table("REGISTRATIONS")]
    public class Registration
    {
        [Key]
        [Column("REG_ID")]
        public int RegId { get; set; }

        [Column("STUDENT_ID")]
        public string StudentId { get; set; } = string.Empty;

        [Column("SECTION_ID")]
        public int SectionId { get; set; }

        [Column("SEMESTER")]
        public string Semester { get; set; } = string.Empty;

        [Column("REG_STATUS")]
        public string RegStatus { get; set; } = "REGISTERED";

        [Column("REGISTERED_AT")]
        public DateTime RegisteredAt { get; set; } = DateTime.Now;

        // Foreign Keys เชื่อมโยงความสัมพันธ์
        [ForeignKey("StudentId")]
        public Student? Student { get; set; }

        [ForeignKey("SectionId")]
        public Section? Section { get; set; }
    }
}