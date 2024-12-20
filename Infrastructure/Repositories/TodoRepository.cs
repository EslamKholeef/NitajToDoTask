using Application.Interfaces;
using Domain.Models;

namespace Infrastructure.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        private readonly List<TodoItem> _todos = new List<TodoItem>();

        public Task<TodoItem> AddAsync(TodoItem todoItem)
        {
            todoItem.Id = _todos.Count + 1; // Eslam: use this while working with in-memory data only.
            _todos.Add(todoItem);
            return Task.FromResult(todoItem);
        }

        public Task<IEnumerable<TodoItem>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<TodoItem>>(_todos);
        }

        public Task<TodoItem> GetByIdAsync(int id)
        {
            var todoItem = _todos.FirstOrDefault(t => t.Id == id);
            return Task.FromResult(todoItem);
        }

        public Task<bool> UpdateAsync(TodoItem todoItem)
        {
            var existingItem = _todos.FirstOrDefault(t => t.Id == todoItem.Id);
            if (existingItem == null)
            {
                return Task.FromResult(false);
            }

            existingItem.Title = todoItem.Title;
            existingItem.Description = todoItem.Description;
            existingItem.IsCompleted = todoItem.IsCompleted;
            return Task.FromResult(true);
        }
    }
}
