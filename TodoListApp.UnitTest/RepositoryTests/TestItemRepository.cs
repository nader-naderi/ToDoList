using Microsoft.EntityFrameworkCore;
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

        [Test]
        public async Task GetById_ExistingId_ReturnsItem()
        {
            var existingItem = new TodoItem { Id = 1, Title = "Existing Task." };

            _dbContext.Setup(db => db.Items.FindAsync(1)).ReturnsAsync(existingItem);

            var result = await _repository.GetById(existingItem.Id);

            Assert.AreEqual(existingItem, result, "Expected item with existing id.");
        }

        [Test]
        public async Task GetById_NonExistentId_ReturnsNull()
        {
            int nonExistentId = 9999;

            _dbContext.Setup(db => db.Items.FindAsync(nonExistentId))
                .ReturnsAsync(null as TodoItem);

            var result = await _repository.GetById(nonExistentId);

            Assert.IsNull(result, "Expected null result for non-existent id.");
        }

        [Test]
        public async Task GetAllAsync_ReturnsAllItems()
        {
            var expectedItems = new List<TodoItem>
            {
                new TodoItem { Id = 1, Title = "Task 1" },
                new TodoItem { Id = 2, Title = "Task 2" },
                new TodoItem { Id = 3, Title = "Task 3" },
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<TodoItem>>();

            mockDbSet.As<IQueryable<TodoItem>>().Setup(m => m.Provider).Returns(expectedItems.Provider);
            mockDbSet.As<IQueryable<TodoItem>>().Setup(m => m.Expression).Returns(expectedItems.Expression);
            mockDbSet.As<IQueryable<TodoItem>>().Setup(m => m.ElementType).Returns(expectedItems.ElementType);
            mockDbSet.As<IQueryable<TodoItem>>().Setup(m => m.GetEnumerator()).Returns(() => expectedItems.GetEnumerator());

            // Define a custom TestAsyncEnumerator for mocking ToListAsync
            var testAsyncEnumerator = new TestAsyncEnumerator<TodoItem>(expectedItems.GetEnumerator());
            mockDbSet.As<IAsyncEnumerable<TodoItem>>()
                .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(testAsyncEnumerator);

            _dbContext.Setup(db => db.Items).Returns(mockDbSet.Object);

            var result = await _repository.GetAllAsync();

            CollectionAssert.AreEqual(expectedItems.ToList(), result.ToList(), "Expected items do not match.");
        }

        [Test]
        public async Task UpdateAsync_ExistingItem_UpdatesItem()
        {
            var existingItem = new TodoItem() { Id = 1, Title = "Existing Task", };

            _dbContext.Setup(db => db.Items.Update(existingItem));

            await _repository.UpdateAsync(existingItem);

            _dbContext.Verify(db => db.SaveChangesAsync(default), Times.Once);
        }

        [Test]
        public async Task DeleteAsync_ExistingItem_DeletesItem()
        {
            var existingItem = new TodoItem { Id = 1, Title = "Existing Task" };

            _dbContext.Setup(db => db.Items.Remove(existingItem));

            await _repository.DeleteAsync(existingItem);

            _dbContext.Verify(db => db.SaveChangesAsync(default), Times.Once);
        }
    }
}
