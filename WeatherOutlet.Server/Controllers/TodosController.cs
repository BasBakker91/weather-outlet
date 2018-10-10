using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeatherOutlet.Data;
using WeatherOutlet.Data.Models;
using WeatherOutlet.Data.Services;

namespace WeatherOutlet.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodosController : ControllerBase
    {
        private readonly ITodoService todoService;

        public TodosController(ITodoService todoService)
        {
            this.todoService = todoService;
        }

        [HttpGet]
        public async Task<List<TodoItem>> GetTodos()
        {
            return await todoService.FindAll();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem([FromRoute] int id, [FromBody] TodoItem todoItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != todoItem.Id)
            {
                return BadRequest();
            }

            todoItem = await todoService.MarkAsCompleted(id);

            return Ok(todoItem);
        }
    }
}