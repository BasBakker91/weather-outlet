using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WeatherOutlet.Data.Models;
using WeatherOutlet.Data.Services;
using Xunit;

namespace WeatherOutlet.Data.Tests
{
    public class TodoServiceTests
    {
        [Fact]
        public async Task FindAll_SimpleFindAllCount()
        {
            // Arrange
            var expected = ApplicationDbContext.GetDefaultTodoItems().Count();
            var service = CreateTodoServiceWithMockDbContext();

            // Act
            var todos = await service.FindAll();
            var actual = todos.Count();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(1, "Create Todo dbcontext")]
        [InlineData(2, "Create Todo Service")]
        [InlineData(3, "Make base class for Service classes")]
        [InlineData(4, "Add more tests")]
        public async Task Find_FindById(int Id, string expected)
        {
            // Arrange
            var service = CreateTodoServiceWithMockDbContext();

            // Act
            var todo = await service.Find(Id);

            // Assert
            Assert.NotNull(todo);
            Assert.Equal(expected, todo.Name);
        }

        [Fact]
        public async Task Find_FindByIdThatDoesNotExist()
        {
            // Arrange
            var service = CreateTodoServiceWithMockDbContext();

            // Act
            var todo = await service.Find(10);

            // Assert
            Assert.Null(todo);
        }

        [Fact]
        public async Task Add_AddTodoItem()
        {
            // Arrange
            var service = CreateTodoServiceWithMockDbContext();
            var todoItemToAdd = new TodoItem()
            {
                Id = 10,
                CreatedAt = DateTime.Now,
                CompletedAt = null,
                Description = "Test Todo Item",
                IsCompleted = false,
                Name = "Test"
            };
            int expected = todoItemToAdd.Id;

            // Act
            await service.Add(todoItemToAdd);
            var todoItem = await service.Find(todoItemToAdd.Id);

            // Assert
            Assert.NotNull(todoItem);
            Assert.Equal(expected, todoItem.Id);
        }

        [Fact]
        public async Task MarkTaskAskCompleted_SimpleMarkTaskAsCompleted()
        {
            // Arrange
            var service = CreateTodoServiceWithMockDbContext();
            bool expected = true;

            // Act
            var todoMarkedAsCompleted = await service.MarkAsCompleted(1);
            var actual = todoMarkedAsCompleted.IsCompleted;

            // Assert
            Assert.NotNull(todoMarkedAsCompleted);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task MarkTaskAskCompleted_SimpleMarkTaskAsCompletedInvalidId()
        {
            // Arrange
            var service = CreateTodoServiceWithMockDbContext();

            // Act
            var actual = await service.MarkAsCompleted(10);

            // Assert
            Assert.Null(actual);
        }

        [Fact]
        public async Task FindAllOpenTodos()
        {
            // Arrange
            var service = CreateTodoServiceWithMockDbContext();

            // Act
            var allOpenTodos = await service.FindAllOpenTodos();

            // Assert
            Assert.True(allOpenTodos.All(i => i.IsCompleted == false));
        }

        // etc...

        #region Arrange utitilities
        private TodoService CreateTodoServiceWithMockDbContext()
        {
            var sourceTodos = ApplicationDbContext.GetDefaultTodoItems().ToList();
            IQueryable<TodoItem> todos = sourceTodos.AsQueryable();

            var mockSet = todos.AsQueryable().BuildMockDbSet();
            mockSet.Setup(_ => _.AddAsync(It.IsAny<TodoItem>(), It.IsAny<CancellationToken>()))
                   .Callback((TodoItem model, CancellationToken token) => { sourceTodos.Add(model); })
                   .Returns((TodoItem model, CancellationToken token) => Task.FromResult((EntityEntry<TodoItem>)null));


            var mockContext = new Mock<ApplicationDbContext>();
            mockContext.Setup(c => c.Todos).Returns(mockSet.Object);

            return new TodoService(mockContext.Object);
        } 
        #endregion
    }
}
