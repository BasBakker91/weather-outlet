using System.Collections.Generic;
using System.Threading.Tasks;
using WeatherOutlet.Data.Models;

namespace WeatherOutlet.Data.Services
{
    public interface ITodoService
    {
        Task Add(TodoItem todoItem);
        Task Delete(int Id);
        Task<TodoItem> Find(int Id);
        Task<List<TodoItem>> FindAll();
        Task<List<TodoItem>> FindAllOpenTodos();
        Task<TodoItem> MarkAsCompleted(int Id);
        Task Update(TodoItem todoItem);
    }
}