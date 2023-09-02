using Microsoft.AspNetCore.Mvc;
using ToDoList.Entities;
using ToDoList.Services;

namespace ToDoList.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ItemController : ControllerBase
    {
        private readonly IService<TodoItem> _itemService;

        public ItemController(IService<TodoItem> itemService)
        {
            _itemService = itemService;
        }

        [HttpPost("PostItem")]
        public async Task<ActionResult<TodoItem>> Post([FromBody] TodoItem item)
        {
            try
            {
                if (item == null)
                    return BadRequest();

                // Use the service to add the item
                await _itemService.AddAsync(item);

                return Ok(item);
            }
            catch (Exception ex)
            {
                // Handle the exception as needed
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet("GetItem/{id}")]
        public async Task<ActionResult<TodoItem>> Get(int id)
        {
            try
            {
                var item = await _itemService.GetById(id);

                if (item == null)
                    return NotFound();

                return Ok(item);
            }
            catch (Exception ex)
            {
                // Handle the exception as needed
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet("GetItems")]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetAll()
        {
            try
            {
                var items = await _itemService.GetAllAsync();
                
                return Ok(items);
            }
            catch (Exception ex)
            {
                // Handle the exception as needed
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}
