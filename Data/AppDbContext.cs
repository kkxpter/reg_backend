using Microsoft.EntityFrameworkCore;
using RegSystemAPI.Models;

namespace RegSystemAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Registration> Registrations { get; set; }

        // 📌 เพิ่มส่วนนี้เพื่อผูกความสัมพันธ์ Foreign Key ให้ถูกต้อง
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // บอกว่า Section 1 อัน เป็นของ Course 1 วิชา โดยอ้างอิงจาก CourseCode
            modelBuilder.Entity<Section>()
                .HasOne(s => s.Course)
                .WithMany() // หรือถ้าระบุ Collection ใน Course ไว้ ก็โยงกันได้
                .HasForeignKey(s => s.CourseCode);

            // ผูก Registration กับ Student และ Section (เผื่อไว้ใช้ในอนาคตด้วยครับ)
            modelBuilder.Entity<Registration>()
                .HasOne(r => r.Student)
                .WithMany()
                .HasForeignKey(r => r.StudentId);

            modelBuilder.Entity<Registration>()
                .HasOne(r => r.Section)
                .WithMany()
                .HasForeignKey(r => r.SectionId);
        }
    }
}