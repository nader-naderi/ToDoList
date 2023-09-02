using Microsoft.EntityFrameworkCore;

using ToDoList.Data;
using ToDoList.Entities;

namespace ToDoList.Repository
{
    public interface IRepository<T>
    {
        Task<T?> GetById(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task DeleteAllAsync();
    }

    public class ItemRepository : IRepository<TodoItem>
    {
        private readonly IItemDbContext _dbContext;

        public ItemRepository(IItemDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TodoItem> AddAsync(TodoItem entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await _dbContext.Items.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task DeleteAllAsync()
        {
            var items = await _dbContext.Items.ToListAsync();
            _dbContext.Items.RemoveRange(items);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(TodoItem entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _dbContext.Items.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<TodoItem>> GetAllAsync()
        {
            return await _dbContext.Items.ToListAsync();
        }

        public async Task<TodoItem?> GetById(int id)
        {
            return await _dbContext.Items.FindAsync(id);
        }

        public async Task UpdateAsync(TodoItem entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _dbContext.SetEntityStateModified(entity);
            await _dbContext.SaveChangesAsync();    
        }
    }
}
