using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RegSystemAPI.Models
{
    [Table("STUDENTS")]
    public class Student
    {
        [Key]
        [Column("STUDENT_ID")]
        public string StudentId { get; set; } = string.Empty;

        [Column("ID_CARD_NO")]
        public string NationalId { get; set; } = string.Empty; // 📌 เปลี่ยนจาก IdCardNo เป็น NationalId เพื่อให้ตรงกับ Angular

        [Column("FIRST_NAME")]
        public string FirstNameTh { get; set; } = string.Empty; // 📌 เปลี่ยนจาก FirstName เป็น FirstNameTh

        [Column("LAST_NAME")]
        public string LastNameTh { get; set; } = string.Empty; // 📌 เปลี่ยนจาก LastName เป็น LastNameTh

        [Column("UNIVERSITY_EMAIL")]
        public string UniversityEmail { get; set; } = string.Empty;

        [Column("PERSONAL_EMAIL")]
        public string? PersonalEmail { get; set; }

        [Column("PHONE_NUMBER")]
        public string? Phone { get; set; } // 📌 เปลี่ยนจาก PhoneNumber เป็น Phone

        [Column("ADDRESS")]
        public string? Province { get; set; } // 📌 เปลี่ยนจาก Address เป็น Province (หรือถ้าใน DB เก็บเป็นจังหวัด)

        [Column("PASSWORD_HASH")]
        public string PasswordHash { get; set; } = string.Empty;

        [Column("GPAX")]
        public decimal Gpax { get; set; }

        [Column("TOTAL_CREDITS")]
        public int TotalCredits { get; set; }

        [Column("STATUS")]
        public string Status { get; set; } = string.Empty;
        [Column("FACULTY")]
        public string? Faculty { get; set; }

        [Column("MAJOR")]
        public string? Major { get; set; }

        [Column("ADVISOR")]
        public string? Advisor { get; set; }
    }
}