using ToDoList.Entities;
using ToDoList.Repository;

namespace ToDoList.Services
{
    public interface IService<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetById(int id);
        Task<TodoItem> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task DeleteAllAsync();
    }

    public class ItemService : IService<TodoItem>
    {
        private readonly IRepository<TodoItem> repository;

        public ItemService(IRepository<TodoItem> repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<IEnumerable<TodoItem>> GetAllAsync()
        {
            return await repository.GetAllAsync();
        }

        public async Task<TodoItem?> GetById(int id)
        {
            return await repository.GetById(id);
        }

        public async Task<TodoItem> AddAsync(TodoItem entity)
        {
            return await repository.AddAsync(entity);
        }

        public async Task UpdateAsync(TodoItem entity)
        {
            await repository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(TodoItem entity)
        {
            await repository.DeleteAsync(entity);
        }

        public async Task DeleteAllAsync()
        {
            await repository.DeleteAllAsync();
        }
    }
}
