using Application.Dtos;
using Application.Interfaces;
using Domain.Models;

namespace Infrastructure.Services
{
    public class TodoService : ITodoService
    {
        private readonly ITodoRepository _todoRepository;

        public TodoService(ITodoRepository todoRepository) => _todoRepository = todoRepository;

        public async Task<TodoItem> CreateTodoAsync(TodoItemDto todoItemDto)
        {
            if (string.IsNullOrWhiteSpace(todoItemDto.Title))
            {
                throw new ArgumentException("Title is required");
            }

            var todoItem = new TodoItem
            {
                Title = todoItemDto.Title,
                Description = todoItemDto.Description
            };

            return await _todoRepository.AddAsync(todoItem);
        }

        public async Task<IEnumerable<TodoItem>> GetAllTodosAsync()
        {
            return await _todoRepository.GetAllAsync();
        }

        public async Task<IEnumerable<TodoItem>> GetPendingTodosAsync()
        {
            var allTodos = await _todoRepository.GetAllAsync();
            return allTodos.Where(t => !t.IsCompleted);
        }

        public async Task<bool> MarkTodoAsCompletedAsync(int id)
        {
            var todoItem = await _todoRepository.GetByIdAsync(id);
            if (todoItem == null)
            {
                return false;
            }

            todoItem.IsCompleted = true;
            return true;
        }
    }
}
