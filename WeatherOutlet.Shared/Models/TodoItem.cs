using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherOutlet.Shared.Models
{
    public class TodoItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CompletedAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
