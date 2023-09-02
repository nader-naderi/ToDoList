using Microsoft.EntityFrameworkCore;

using ToDoList.Entities;

namespace ToDoList.Data
{
    public class ItemDbContext : DbContext
    {
        public ItemDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<TodoItem> Items { get; private set; }
    }
}
