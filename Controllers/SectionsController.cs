using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegSystemAPI.Data;
using RegSystemAPI.Models;

namespace RegSystemAPI.Controllers
{
    [Route("api/[controller]")]
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
            // 📌 เอา .Include ออก เพื่อป้องกันการพัง
            return await _context.Sections.ToListAsync();
        }
    }
}