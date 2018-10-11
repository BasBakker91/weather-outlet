using Microsoft.EntityFrameworkCore;
using WeatherOutlet.Data.Models;

namespace WeatherOutlet.Data
{
    public interface IApplicationDbContext
    {
        DbSet<TodoItem> Todos { get; set; }
    }
}