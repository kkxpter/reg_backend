using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegSystemAPI.Data;
using RegSystemAPI.Models;

namespace RegSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CoursesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/courses (ดึงรายวิชาทั้งหมด 20 วิชา)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourses()
        {
            return await _context.Courses.ToListAsync();
        }

        // GET: api/courses/0502301 (ดึงรายวิชาตามรหัสวิชา)
        [HttpGet("{code}")]
        public async Task<ActionResult<Course>> GetCourse(string code)
        {
            var course = await _context.Courses.FindAsync(code);

            if (course == null)
            {
                return NotFound();
            }

            return course;
        }
    }
}