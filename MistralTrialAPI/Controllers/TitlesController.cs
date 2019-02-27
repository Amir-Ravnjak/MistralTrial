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
    public class TitlesController : ControllerBase
    {
        private readonly MyContext _context;

        public TitlesController(MyContext context)
        {
            _context = context;
        }

        // GET: api/Titles
        [HttpGet]
        public IEnumerable<Title> GetTitles()
        {
                return _context.Titles;
            
        }

        // GET: api/Titles/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTitle([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var title = await _context.Titles.FindAsync(id);

            if (title == null)
            {
                return NotFound();
            }

            return Ok(title);
        }

        // PUT: api/Titles/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutTitle([FromRoute] int id, [FromBody] Title title)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != title.Id)
            {
                return BadRequest();
            }

            _context.Entry(title).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TitleExists(id))
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

        // POST: api/Titles
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PostTitle([FromBody] Title title)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Titles.Add(title);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTitle", new { id = title.Id }, title);
        }

        // DELETE: api/Titles/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteTitle([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var title = await _context.Titles.FindAsync(id);
            if (title == null)
            {
                return NotFound();
            }

            _context.Titles.Remove(title);
            await _context.SaveChangesAsync();

            return Ok(title);
        }

        private bool TitleExists(int id)
        {
            return _context.Titles.Any(e => e.Id == id);
        }
    }
}