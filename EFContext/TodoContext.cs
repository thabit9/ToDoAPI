using Microsoft.EntityFrameworkCore;
using TodoAPI.Models.BusinessModels;

namespace TodoAPI.EFContext
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options): base(options)
        {            
        }
        public DbSet<TodoItem> TodoItems { get; set; }
    }
}