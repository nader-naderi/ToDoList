using Microsoft.EntityFrameworkCore;

using ToDoList.Entities;

namespace ToDoList.Data
{
    public interface IItemDbContext
    {
        DbSet<TodoItem> Items { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        void SetEntityStateModified<T>(T entity) where T : class;
    }

    public class ItemDbContext : DbContext, IItemDbContext
    {
        public ItemDbContext()
        {
            
        }

        public ItemDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<TodoItem> Items { get; private set; }

        public void SetEntityStateModified<T>(T entity) where T : class
        {
            Entry(entity).State = EntityState.Modified;
        }
    }
}
