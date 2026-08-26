using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegSystemAPI.Data;
using RegSystemAPI.Contracts;
using RegSystemAPI.Services;

namespace RegSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly JwtTokenService _jwtTokenService;

        public AuthController(AppDbContext context, JwtTokenService jwtTokenService)
        {
            _context = context;
            _jwtTokenService = jwtTokenService;
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
                accessToken = _jwtTokenService.CreateToken(student),
                student = StudentProfileDto.FromStudent(student)
            });
        }
    }
}
