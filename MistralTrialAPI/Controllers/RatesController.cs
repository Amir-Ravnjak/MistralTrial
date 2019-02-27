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
    public class RatesController : ControllerBase
    {
        private readonly MyContext _context;

        public RatesController(MyContext context)
        {
            _context = context;
        }

        // GET: api/Rates
        [HttpGet]
        public IEnumerable<Rate> GetRates()
        {
            return _context.Rates;
        }

        // GET: api/Rates/5
        [HttpGet("{titleId}/{userId}")]
        public async Task<IActionResult> GetRate([FromRoute] int titleId,[FromRoute]int userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var rate = _context.Rates.Where(x => x.TitleId == titleId && x.UserId == userId).FirstOrDefault();

            if (rate == null)
            {
                return NotFound();
            }

            return Ok(rate);
        }

        // PUT: api/Rates/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRate([FromRoute] int id, [FromBody] Rate rate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != rate.Id)
            {
                return BadRequest();
            }

            _context.Entry(rate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RateExists(id))
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

        // POST: api/Rates
        [HttpPost]
        public async Task<IActionResult> PostRate([FromBody] Rate rate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Rates.Add(rate);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRate", new { id = rate.Id }, rate);
        }

        // DELETE: api/Rates/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRate([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var rate = await _context.Rates.FindAsync(id);
            if (rate == null)
            {
                return NotFound();
            }

            _context.Rates.Remove(rate);
            await _context.SaveChangesAsync();

            return Ok(rate);
        }

        private bool RateExists(int id)
        {
            return _context.Rates.Any(e => e.Id == id);
        }
    }
}