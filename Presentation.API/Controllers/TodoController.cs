using Application.Dtos;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly ITodoService _todoService;

        public TodoController(ITodoService todoService) => _todoService = todoService;

        [HttpPost]
        public async Task<IActionResult> CreateTodo([FromBody] TodoItemDto todoItemDto)
        {
            var todoItem = await _todoService.CreateTodoAsync(todoItemDto);
            return CreatedAtAction(nameof(GetAllTodos), new { id = todoItem.Id }, todoItem);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTodos()
        {
            var todos = await _todoService.GetAllTodosAsync();
            return Ok(todos);
        }

        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingTodos()
        {
            var todos = await _todoService.GetPendingTodosAsync();
            return Ok(todos);
        }

        [HttpPut("{id}/complete")]
        public async Task<IActionResult> MarkTodoAsCompleted(int id)
        {
            var success = await _todoService.MarkTodoAsCompletedAsync(id);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
