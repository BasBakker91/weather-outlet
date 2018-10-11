using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherOutlet.Data.Models;

namespace WeatherOutlet.Data.Services
{
    public class TodoService : ITodoService
    {
        private readonly ApplicationDbContext context;

        public TodoService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<TodoItem> Find(int Id)
        {
            return await context.Todos.FirstOrDefaultAsync(i => i.Id == Id);
        }

        public async Task<List<TodoItem>> FindAll()
        {
            return await context.Todos.OrderBy(i => i.CreatedAt).ToListAsync();
        }

        public async Task Add(TodoItem todoItem)
        {
            await context.Todos.AddAsync(todoItem);
            await context.SaveChangesAsync();
        }

        public async Task Update(TodoItem todoItem)
        {
            context.Todos.Update(todoItem);
            await context.SaveChangesAsync();
        }

        public async Task Delete(int Id)
        {
            var todo = await Find(Id);

            if (todo == null)
                return;

            context.Todos.Remove(todo);
            await context.SaveChangesAsync();
        }

        public async Task<TodoItem> MarkAsCompleted(int Id)
        {
            var todoItem = await Find(Id);

            if (todoItem == null)
                return null;

            todoItem.IsCompleted = true;
            todoItem.CompletedAt = DateTime.Now;

            await context.SaveChangesAsync();

            return todoItem;
        }

        public async Task<List<TodoItem>> FindAllOpenTodos()
        {
            return await context.Todos.Where(i => i.IsCompleted == false).OrderBy(i => i.CreatedAt).ToListAsync();
        }
    }
}
