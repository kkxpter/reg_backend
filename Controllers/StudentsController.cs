using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegSystemAPI.Data;
using RegSystemAPI.Models;

namespace RegSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StudentsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/students
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            // ดึงข้อมูลนิสิตทั้งหมดจาก Oracle Database
            return await _context.Students.ToListAsync();
        }

        // GET: api/students/6601234567
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(string id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            return student;
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(string id, [FromBody] Student updatedStudent)
        {
            // 1. หาข้อมูลนักศึกษาเดิมจาก Database
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound(new { message = "ไม่พบข้อมูลนักศึกษา" });
            }

            // 2. อัปเดตค่าฟิลด์ต่างๆ (เช็กให้แน่ใจว่า mapping ครบทุกช่อง)
            student.FirstNameTh = updatedStudent.FirstNameTh;
            student.LastNameTh = updatedStudent.LastNameTh;
            student.UniversityEmail = updatedStudent.UniversityEmail;
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
                if (!_context.Students.Any(e => e.StudentId == id))
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

        private bool StudentExists(string id)
        {
            return _context.Students.Any(e => e.StudentId == id);
        }
        
    }
    
}