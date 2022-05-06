using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoAPI.EFContext;
using TodoAPI.Models.BusinessModels;
using TodoAPI.Models.DTOModels;


//A DTO may be used to:

//Prevent over-posting.
//Hide properties that clients are not supposed to view.
//Omit some properties in order to reduce payload size.
//Flatten object graphs that contain nested objects. Flattened object graphs can be more convenient for clients.
//Update the TodoItemsController to use TodoItemDTO: - for that i have created a new controller
//TodoItemsDTOController but in real projects we update the same controller instead of creating a new one
namespace TodoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsDTOController : ControllerBase
    {
        private readonly TodoContext _context;

        public TodoItemsDTOController(TodoContext context)
        {
            _context = context;
        }

        // GET: api/TodoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetTodoItemsx()
        {
            return await _context.TodoItems
                    .Select(x => ItemToDTO(x))
                    .ToListAsync();
                    
        }

        // GET: api/TodoItems/5
        [HttpGet("{id}", Name ="GetTodoItemx")]
        public async Task<ActionResult<TodoItemDTO>> GetTodoItemx(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return ItemToDTO(todoItem);
        }

        // PUT: api/TodoItems/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodoItem(long id, TodoItemDTO todoItemDTO)
        {
            if (id != todoItemDTO.Id)
            {
                return BadRequest();
            }

            //_context.Entry(todoItem).State = EntityState.Modified;
            var todoItem = await _context.TodoItems.FindAsync(id);
            if(todoItem == null)
            {
                return NotFound();
            }

            todoItem.Name = todoItemDTO.Name;
            todoItem.IsComplete = todoItemDTO.IsComplete;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!TodoItemExists(id))
            {
                return NotFound();
                //if i dont want to use the "when" key
                //this is an altenate way
                /*if (!TodoItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }*/
            }

            return NoContent();
        }

        // POST: api/TodoItems
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<TodoItemDTO>> CreateTodoItemx(TodoItemDTO todoItemDTO)
        {
            var todoItem = new TodoItem
            {
                IsComplete = todoItemDTO.IsComplete,
                Name = todoItemDTO.Name
            };

            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetTodoItemx), 
                new { id = todoItem.Id }, 
                ItemToDTO(todoItem));
        }

        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TodoItemExists(long id) =>
                _context.TodoItems.Any(e => e.Id == id);
       
        
        
        private static TodoItemDTO ItemToDTO(TodoItem todoItem) =>
                new TodoItemDTO
                {
                    Id = todoItem.Id,
                    Name = todoItem.Name,
                    IsComplete = todoItem.IsComplete
                };
    }
}
