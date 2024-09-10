using Microsoft.AspNetCore.Mvc;
using kish_insurance_service.Models;
using Microsoft.EntityFrameworkCore;

namespace kish_insurance_service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoverageTypesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CoverageTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/CoverageTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CoverageType>>> GetCoverageTypes()
        {
            return await _context.CoverageTypes.ToListAsync();
        }

        // GET: api/CoverageTypes/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CoverageType>> GetCoverageType(int id)
        {
            var coverageType = await _context.CoverageTypes.FindAsync(id);

            if (coverageType == null)
            {
                return NotFound();
            }

            return coverageType;
        }

        // POST: api/CoverageTypes
        [HttpPost]
        public async Task<ActionResult<CoverageType>> PostCoverageType(CoverageType coverageType)
        {
            _context.CoverageTypes.Add(coverageType);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCoverageType), new { id = coverageType.Id }, coverageType);
        }

        // PUT: api/CoverageTypes/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCoverageType(int id, CoverageType coverageType)
        {
            if (id != coverageType.Id)
            {
                return BadRequest();
            }

            _context.Entry(coverageType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CoverageTypeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/CoverageTypes/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCoverageType(int id)
        {
            var coverageType = await _context.CoverageTypes.FindAsync(id);
            if (coverageType == null)
            {
                return NotFound();
            }

            _context.CoverageTypes.Remove(coverageType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CoverageTypeExists(int id)
        {
            return _context.CoverageTypes.Any(e => e.Id == id);
        }
    }
}
