using JWT_API.Data;
using JWT_API.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace JWT_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TodoController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public TodoController(ApiDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetItems()
        {
            var items = await _context.Items.ToListAsync();
            if (items == null)
                return NotFound();
            return Ok(items);

        }

        [HttpPost]
        public async Task<IActionResult> CreateItem(ItemData model)
        {
            if (ModelState.IsValid)
            {
                await _context.Items.AddAsync(model);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetItem", new { model.Id }, model);
            }

            return new JsonResult("Something Went wrong") { StatusCode = 500 };
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetItem(int id)
        {
            var item = await _context.Items.FirstOrDefaultAsync(x => x.Id == id);
            if (item == null)
                return NotFound();
            return Ok(item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult>UpdateItem(int id,ItemData model)
        {
            if(id != model.Id)
                return BadRequest();

            var existItem = await _context.Items.FirstOrDefaultAsync(x => x.Id == id);
            if (existItem == null)
                return NotFound();
            existItem.Title = model.Title;
            existItem.Description = model.Description;
            existItem.Done = model.Done;

            await _context.SaveChangesAsync();

            return NoContent();

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var existItem = await _context.Items.FirstOrDefaultAsync(x => x.Id==id);

            if (existItem == null)
                return NotFound();
            _context.Items.Remove(existItem);
            await _context.SaveChangesAsync();

            return Ok(existItem);
        }
    }   

}
