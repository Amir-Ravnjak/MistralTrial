using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MistralTrialAPI.Data;

namespace MistralTrialAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TitleActorsController : ControllerBase
    {
        private readonly MyContext _context;

        public TitleActorsController(MyContext context)
        {
            _context = context;
        }

        // GET: api/TitleActors
        [HttpGet("{titleId}")]
        public IEnumerable<TitleActors> GetTitleActors(int titleId)
        {
            return _context.TitleActors.Where(w=>w.TitleId==titleId);
        }

        

        // PUT: api/TitleActors/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTitleActors([FromRoute] int id, [FromBody] TitleActors titleActors)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != titleActors.Id)
            {
                return BadRequest();
            }

            _context.Entry(titleActors).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TitleActorsExists(id))
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

        // POST: api/TitleActors
        [HttpPost]
        public async Task<IActionResult> PostTitleActors([FromBody] TitleActors titleActors)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.TitleActors.Add(titleActors);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTitleActors", new { id = titleActors.Id }, titleActors);
        }

        // DELETE: api/TitleActors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTitleActors([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var titleActors = await _context.TitleActors.FindAsync(id);
            if (titleActors == null)
            {
                return NotFound();
            }

            _context.TitleActors.Remove(titleActors);
            await _context.SaveChangesAsync();

            return Ok(titleActors);
        }

        private bool TitleActorsExists(int id)
        {
            return _context.TitleActors.Any(e => e.Id == id);
        }
    }
}