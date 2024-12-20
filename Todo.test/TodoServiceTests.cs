namespace Todo.test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Application.Dtos;
    using Application.Interfaces;
    using Domain.Models;
    using Infrastructure.Services;
    using Moq;
    using Xunit;

    public class TodoServiceTests
    {
        private readonly ITodoService _todoService;
        private readonly Mock<ITodoRepository> _todoRepositoryMock;

        public TodoServiceTests()
        {
            _todoRepositoryMock = new Mock<ITodoRepository>();
            _todoService = new TodoService(_todoRepositoryMock.Object);
        }

        [Fact]
        public async Task CreateTodoAsync_ShouldThrowException_WhenTitleIsEmpty()
        {
            var todoItemDto = new TodoItemDto { Title = "" };

            await Assert.ThrowsAsync<ArgumentException>(() => _todoService.CreateTodoAsync(todoItemDto));
        }

        [Fact]
        public async Task CreateTodoAsync_ShouldReturnTodoItem_WhenValidInput()
        {
            var todoItemDto = new TodoItemDto { Title = "Test Todo" };
            var expectedTodoItem = new TodoItem { Id = 1, Title = "Test Todo", CreatedDate = DateTime.UtcNow };

            _todoRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<TodoItem>()))
                .ReturnsAsync(expectedTodoItem);

            var result = await _todoService.CreateTodoAsync(todoItemDto);

            Assert.NotNull(result);
            Assert.Equal(expectedTodoItem.Title, result.Title);
        }

        [Fact]
        public async Task GetAllTodosAsync_ShouldReturnAllTodos()
        {
            var todos = new List<TodoItem>
        {
            new TodoItem { Id = 1, Title = "Todo 1" },
            new TodoItem { Id = 2, Title = "Todo 2" }
        };

            _todoRepositoryMock.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(todos);

            var result = await _todoService.GetAllTodosAsync();

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetPendingTodosAsync_ShouldReturnOnlyPendingTodos()
        {
            var todos = new List<TodoItem>
        {
            new TodoItem { Id = 1, Title = "Todo 1", IsCompleted = false },
            new TodoItem { Id = 2, Title = "Todo 2", IsCompleted = true }
        };

            _todoRepositoryMock.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(todos);

            var result = await _todoService.GetPendingTodosAsync();

            Assert.Single(result);
            Assert.False(result.First().IsCompleted);
        }

        [Fact]
        public async Task MarkTodoAsCompletedAsync_ShouldReturnTrue_WhenTodoExists()
        {
            var todoItem = new TodoItem { Id = 1, Title = "Todo 1", IsCompleted = false };

            _todoRepositoryMock.Setup(repo => repo.GetByIdAsync(todoItem.Id))
                .ReturnsAsync(todoItem);

            var result = await _todoService.MarkTodoAsCompletedAsync(todoItem.Id);

            Assert.True(result);
            Assert.True(todoItem.IsCompleted);
        }

        [Fact]
        public async Task MarkTodoAsCompletedAsync_ShouldReturnFalse_WhenTodoDoesNotExist()
        {
            _todoRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((TodoItem)null);

            var result = await _todoService.MarkTodoAsCompletedAsync(1);

            Assert.False(result);
        }
    }
}