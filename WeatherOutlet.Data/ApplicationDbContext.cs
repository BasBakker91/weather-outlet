using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WeatherOutlet.Data.Models;

namespace WeatherOutlet.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<TodoItem> Todos { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TodoItem>().HasData(GetDefaultTodoItems());
        }

        private TodoItem[] GetDefaultTodoItems()
        {
            return new TodoItem[]
            {
                new TodoItem
                {
                    Id = 1,
                    Name = "Create Todo dbcontext",
                    Description = "Create a todo context persist your todos",
                    CreatedAt = DateTime.Now.AddDays(-1).AddHours(-5),
                    CompletedAt = DateTime.Now.AddHours(-1),
                    IsCompleted = true
                },
                new TodoItem
                {
                    Id = 2,
                    Name = "Create Todo Service",
                    Description = "Create a service to perform CRUD operations on the database",
                    CreatedAt = DateTime.Now.AddDays(-1).AddHours(-3),
                    CompletedAt = DateTime.Now.AddHours(1),
                    IsCompleted = true
                },
                new TodoItem
                {
                    Id = 3,
                    Name = "Make base class for Service classes",
                    Description = "Create base classes voor EntityService classes to make your live easier in the long run!",
                    CreatedAt = DateTime.Now.AddDays(-1).AddHours(-1),
                    CompletedAt = null,
                    IsCompleted = false
                },
                new TodoItem
                {
                    Id = 4,
                    Name = "Add more tests",
                    Description = "Add loads of more tests!",
                    CreatedAt = DateTime.Now.AddDays(-1).AddHours(-1),
                    CompletedAt = null,
                    IsCompleted = false
                }
            };
        }
    }
}
