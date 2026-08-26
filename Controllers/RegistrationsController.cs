using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using RegSystemAPI.Data;
using RegSystemAPI.Models;
using RegSystemAPI.Contracts;

namespace RegSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RegistrationsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RegistrationsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/registrations (ดูประวัติการลงทะเบียนทั้งหมดพร้อมข้อมูลนิสิตและกลุ่มเรียน)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RegistrationDto>>> GetRegistrations()
        {
            var studentId = GetStudentId();
            var registrations = await _context.Registrations
                .Where(r => r.StudentId == studentId)
                .Include(r => r.Student)
                .Include(r => r.Section)
                .ThenInclude(s => s!.Course)
                .ToListAsync();
            return registrations.Select(RegistrationDto.FromRegistration).ToList();
        }

        // POST: api/registrations
        // POST: api/registrations
        [HttpPost]
        public async Task<IActionResult> RegisterCourse([FromBody] RegisterRequestDto request)
        {
            // 1. ตรวจสอบว่านิสิตมีตัวตนจริงไหม
            var studentId = GetStudentId();
            var student = await _context.Students.FindAsync(studentId);
            if (student == null)
            {
                return NotFound(new { message = "ไม่พบรหัสนิสิตนี้ในระบบ" });
            }

            // 2. ตรวจสอบว่ากลุ่มเรียน (Section) มีอยู่จริงไหม พร้อมดึงข้อมูลวิชา
            var section = await _context.Sections
                .Include(s => s.Course)
                .FirstOrDefaultAsync(s => s.SectionId == request.SectionId);
                
            if (section == null)
            {
                return NotFound(new { message = "ไม่พบกลุ่มเรียนที่ต้องการลงทะเบียน" });
            }

            // 3. ตรวจสอบว่านิสิตเคยลงทะเบียนกลุ่มเรียนนี้ไปแล้วหรือยัง (Sec เดิม)
            var alreadyRegistered = await _context.Registrations
                .AnyAsync(r => r.StudentId == studentId && r.SectionId == request.SectionId && r.RegStatus == "REGISTERED");

            if (alreadyRegistered)
            {
                return BadRequest(new { message = "นิสิตได้ลงทะเบียนในกลุ่มเรียนนี้ไปแล้ว" });
            }

            // 📌 4. [เพิ่มตรงนี้] ตรวจสอบว่าในเทอมนี้ นิสิตเคยลงทะเบียน "รายวิชานี้" ไปแล้วหรือยัง (แม้จะคนละ Sec)
            var alreadyRegisteredCourse = await _context.Registrations
                .Include(r => r.Section)
                .AnyAsync(r => r.StudentId == studentId && 
                               r.Semester == request.Semester && 
                               r.RegStatus == "REGISTERED" && 
                               r.Section!.CourseCode == section.CourseCode);

            if (alreadyRegisteredCourse)
            {
                return BadRequest(new { message = $"วิชา {section.CourseCode}: นิสิตได้ลงทะเบียนรายวิชานี้ไปแล้วในภาคเรียนเดียวกัน (ไม่สามารถเรียนซ้ำต่างกลุ่มเรียนได้)" });
            }

            // 5. ตรวจสอบว่าที่นั่งเต็มหรือยัง (Enrolled >= Capacity)
            if (section.Enrolled >= section.Capacity)
            {
                return BadRequest(new { message = "กลุ่มเรียนนี้เต็มแล้ว ไม่สามารถลงทะเบียนได้" });
            }

            // 6. ทำการบันทึกการลงทะเบียน
            var registration = new Registration
            {
                StudentId = studentId,
                SectionId = request.SectionId,
                Semester = request.Semester,
                RegStatus = "REGISTERED",
                RegisteredAt = DateTime.Now
            };

            _context.Registrations.Add(registration);

            // 7. อัปเดตจำนวนคนลงทะเบียนใน Section เพิ่มขึ้น 1 คน
            section.Enrolled += 1;
            _context.Sections.Update(section);

            await _context.SaveChangesAsync();

            return Ok(new { message = "ลงทะเบียนเรียนสำเร็จ!", data = registration });
        }

        // 📌 7. เพิ่มฟังก์ชัน DELETE: api/registrations/{id} (สำหรับถอนรายวิชา) ตรงนี้
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRegistration(int id)
        {
            // ค้นหาข้อมูลการลงทะเบียนจาก ID (RegId) พร้อมดึงข้อมูล Section มาด้วยเพื่อลดจำนวนคนเรียนลง
            var registration = await _context.Registrations
                .Include(r => r.Section)
                .FirstOrDefaultAsync(r => r.RegId == id);

            if (registration == null)
            {
                return NotFound(new { message = "ไม่พบข้อมูลการลงทะเบียนนี้ในระบบ" });
            }

            if (registration.StudentId != GetStudentId())
            {
                return Forbid();
            }

            // ถ้าเจอ Section ให้ลดจำนวนคนลงทะเบียน (Enrolled) ลง 1 คน
            if (registration.Section != null)
            {
                registration.Section.Enrolled = Math.Max(0, registration.Section.Enrolled - 1);
                _context.Sections.Update(registration.Section);
            }

            // ลบข้อมูลการลงทะเบียนออกจากฐานข้อมูล
            _context.Registrations.Remove(registration);
            await _context.SaveChangesAsync();

            return Ok(new { message = "ถอนรายวิชาสำเร็จ" });
        }

    // DTO สำหรับรับข้อมูลตอนกดลงทะเบียน
    public class RegisterRequestDto
    {
        public int SectionId { get; set; }
        public string Semester { get; set; } = "1/2569";
    }

    private string GetStudentId()
    {
        return User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new UnauthorizedAccessException("Student identity claim is missing.");
    }
    }
}
