using Microsoft.EntityFrameworkCore.ChangeTracking;

using Moq;

using NUnit.Framework;

using ToDoList.Data;
using ToDoList.Entities;
using ToDoList.Repository;

namespace TodoListApp.UnitTest.RepositoryTests
{
    [TestFixture]
    internal class TestItemRepository
    {
        private ItemRepository _repository;
        private Mock<IItemDbContext> _dbContext;

        [SetUp]
        public void SetUp()
        {
            _dbContext = new Mock<IItemDbContext>();

            _repository = new ItemRepository(_dbContext.Object);
        }

        [Test]
        public async Task AddAsync_ValidItem_ReturnsAddedItem()
        {
            var newItem = new TodoItem { Id = 1, Title = "New Task" };

            _dbContext.Setup(db => db.Items.AddAsync(newItem, default))
                .Returns(new ValueTask<EntityEntry<TodoItem>>(Task.FromResult<EntityEntry<TodoItem>>(null)));

            var result = await _repository.AddAsync(newItem);

            Assert.AreEqual(newItem, result, "Expected added item to be returned.");
        }
    }
}
