using Application.Dtos;
using Domain.Models;

namespace Application.Interfaces
{
    public interface ITodoService
    {
        Task<TodoItem> CreateTodoAsync(TodoItemDto todoItemDto);
        Task<IEnumerable<TodoItem>> GetAllTodosAsync();
        Task<IEnumerable<TodoItem>> GetPendingTodosAsync();
        Task<bool> MarkTodoAsCompletedAsync(int id);
    }
}
