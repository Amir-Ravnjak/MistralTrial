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
    public class ImgFilesController : ControllerBase
    {
        private readonly MyContext _context;

        public ImgFilesController(MyContext context)
        {
            _context = context;
        }

        // GET: api/ImgFiles
        [HttpGet]
        public IEnumerable<ImgFile> GetImgFiles()
        {
            return _context.ImgFiles;
        }

        // GET: api/ImgFiles/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetImgFile([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var imgFile = await _context.ImgFiles.FindAsync(id);

            if (imgFile == null)
            {
                return NotFound();
            }

            return Ok(imgFile);
        }

        // PUT: api/ImgFiles/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutImgFile([FromRoute] int id, [FromBody] ImgFile imgFile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != imgFile.Id)
            {
                return BadRequest();
            }

            _context.Entry(imgFile).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ImgFileExists(id))
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

        // POST: api/ImgFiles
        [HttpPost]
        public async Task<IActionResult> PostImgFile([FromBody] ImgFile imgFile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.ImgFiles.Add(imgFile);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetImgFile", new { id = imgFile.Id }, imgFile);
        }

        // DELETE: api/ImgFiles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImgFile([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var imgFile = await _context.ImgFiles.FindAsync(id);
            if (imgFile == null)
            {
                return NotFound();
            }

            _context.ImgFiles.Remove(imgFile);
            await _context.SaveChangesAsync();

            return Ok(imgFile);
        }

        private bool ImgFileExists(int id)
        {
            return _context.ImgFiles.Any(e => e.Id == id);
        }
    }
}