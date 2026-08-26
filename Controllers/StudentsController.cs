using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using RegSystemAPI.Data;
using RegSystemAPI.Contracts;

namespace RegSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StudentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StudentsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("me")]
        public async Task<ActionResult<StudentProfileDto>> GetMyProfile()
        {
            var student = await _context.Students.FindAsync(GetStudentId());

            if (student == null)
            {
                return NotFound();
            }

            return StudentProfileDto.FromStudent(student);
        }
        [HttpPut("me")]
        public async Task<IActionResult> UpdateMyProfile([FromBody] UpdateStudentProfileDto updatedStudent)
        {
            // 1. หาข้อมูลนักศึกษาเดิมจาก Database
            var student = await _context.Students.FindAsync(GetStudentId());
            if (student == null)
            {
                return NotFound(new { message = "ไม่พบข้อมูลนักศึกษา" });
            }

            // 2. อัปเดตค่าฟิลด์ต่างๆ (เช็กให้แน่ใจว่า mapping ครบทุกช่อง)
            student.PersonalEmail = updatedStudent.PersonalEmail;
            student.Phone = updatedStudent.Phone;
            student.Province = updatedStudent.Province;
            // ...อัปเดตฟิลด์อื่นๆ ตามที่มีใน Model ของพี่...

            try
            {
                // 3. บันทึกลง Database จริงๆ
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Students.Any(e => e.StudentId == GetStudentId()))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent(); // หรือ return Ok(new { message = "บันทึกสำเร็จ" });
        }

        private string GetStudentId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? throw new UnauthorizedAccessException("Student identity claim is missing.");
        }
        
    }
    
}
