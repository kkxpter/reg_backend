using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegSystemAPI.Data;

namespace RegSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        // DTO สำหรับรับข้อมูล Login
        public class LoginRequestDto
        {
            public string StudentId { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }

        // POST: api/auth/login
        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            // ค้นหานิสิตจากรหัสนิสิต
            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.StudentId == request.StudentId);

            if (student == null)
            {
                return Unauthorized(new { message = "ไม่พบรหัสนิสิตนี้ในระบบ" });
            }

            // ตรวจสอบรหัสผ่าน
            if (student.PasswordHash != request.Password)
            {
                return Unauthorized(new { message = "รหัสผ่านไม่ถูกต้อง" });
            }

            // 📌 ปรับแก้ตรงนี้ให้ชื่อ property ตรงกับ Model ล่าสุด (FirstNameTh, LastNameTh)
            return Ok(new
            {
                message = "เข้าสู่ระบบสำเร็จ!",
                student = new
                {
                    student.StudentId,
                    FirstNameTh = student.FirstNameTh, // เปลี่ยนให้ตรงกับ Model ใหม่
                    LastNameTh = student.LastNameTh,   // เปลี่ยนให้ตรงกับ Model ใหม่
                    student.UniversityEmail,
                    student.Gpax,
                    student.TotalCredits,
                    student.Status
                }
            });
        }
    }
}