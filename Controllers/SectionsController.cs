using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegSystemAPI.Data;
using RegSystemAPI.Models;

namespace RegSystemAPI.Controllers
{
    [Route("api/[controller]")] // จะกลายเป็น api/sections อัตโนมัติ
    [ApiController]
    public class SectionsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SectionsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/sections
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Section>>> GetSections()
        {
            return await _context.Sections
                .Include(s => s.Course) // 📌 สำคัญมาก ต้องดึงข้อมูล Course มาพร้อมกันด้วย
                .ToListAsync();
        }
    }
}