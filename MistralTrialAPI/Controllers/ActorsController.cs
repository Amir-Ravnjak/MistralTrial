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
    public class ActorsController : ControllerBase
    {
        private readonly MyContext _context;

        public ActorsController(MyContext context)
        {
            _context = context;
        }

        // GET: api/Actors
        [HttpGet]
        public IEnumerable<Actors> GetActors()
        {
            return _context.Actors;
        }

        // GET: api/Actors/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetActors([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var actors = await _context.Actors.FindAsync(id);

            if (actors == null)
            {
                return NotFound();
            }

            return Ok(actors);
        }

        // PUT: api/Actors/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutActors([FromRoute] int id, [FromBody] Actors actors)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != actors.Id)
            {
                return BadRequest();
            }

            _context.Entry(actors).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ActorsExists(id))
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

        // POST: api/Actors
        [HttpPost]
        public async Task<IActionResult> PostActors([FromBody] Actors actors)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Actors.Add(actors);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetActors", new { id = actors.Id }, actors);
        }

        // DELETE: api/Actors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActors([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var actors = await _context.Actors.FindAsync(id);
            if (actors == null)
            {
                return NotFound();
            }

            _context.Actors.Remove(actors);
            await _context.SaveChangesAsync();

            return Ok(actors);
        }

        private bool ActorsExists(int id)
        {
            return _context.Actors.Any(e => e.Id == id);
        }
    }
}