using Domain.Models;

namespace Application.Interfaces
{
    public interface ITodoRepository
    {
        Task<TodoItem> AddAsync(TodoItem todoItem);
        Task<IEnumerable<TodoItem>> GetAllAsync();
        Task<TodoItem> GetByIdAsync(int id);
        Task<bool> UpdateAsync(TodoItem todoItem);
    }
}
