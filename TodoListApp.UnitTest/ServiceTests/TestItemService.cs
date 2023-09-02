using Moq;
using NUnit.Framework;
using ToDoList.Entities;
using ToDoList.Repository;
using ToDoList.Services;

namespace TodoListApp.UnitTest.ServiceTests
{
    [TestFixture]
    internal class TestItemService
    {
        private ItemService _service;
        private Mock<IRepository<TodoItem>> _repositoryMock;

        [SetUp]
        public void Setup()
        {
            _repositoryMock = new Mock<IRepository<TodoItem>>();
            _service = new ItemService(_repositoryMock.Object);
        }

        [Test]
        public async Task GetAll_ReturnsItems()
        {
            var expectedItems = new List<TodoItem>()
            {
                new TodoItem() { Id = 1, Title = "Test Task 1", },
                new TodoItem() { Id = 1, Title = "Test Task 2", }
            };

            _repositoryMock.Setup(repo => repo.GetAllAsync()).
                ReturnsAsync(expectedItems);

            var result = await _service.GetAllAsync();

            CollectionAssert.AreEqual(expectedItems, result, "Expected items do not match.");
        }

        [Test]
        [TestCase(1, "Test Task 1")]
        [TestCase(2, "Test Task 2")]
        [TestCase(3, "Test Task 3")]
        public async Task GetById_ValidId_ReturnsItem(int id, string title)
        {
            var expectedItems = new TodoItem() { Id = id, Title = title };

            _repositoryMock.Setup(repo => repo.GetById(expectedItems.Id)).ReturnsAsync(expectedItems);

            var result = await _service.GetById(expectedItems.Id);

            Assert.AreEqual(expectedItems, result, "Expected item does not match.");
        }

        [Test]
        public async Task GetById_NonExistenceId_ReturnsNull()
        {
            int nonExistentId = 9999;

            _repositoryMock.Setup(repo => repo.GetById(nonExistentId)).ReturnsAsync(value: null);

            var result = await _service.GetById(nonExistentId);

            Assert.IsNull(result, "Expected null result for non-existent ID.");
        }

        [Test]
        public async Task AddAsync_ValidItem_ReturnsAddedItem()
        {
            var newItem = new TodoItem { Id = 4, Title = "New Task" };

            _repositoryMock.Setup(repo => repo.AddAsync(newItem)).ReturnsAsync(newItem);

            var result = await _service.AddAsync(newItem);

            Assert.AreEqual(newItem, result, "Expected added item to be returned.");
        }

        [Test]
        public async Task UpdateAsync_Existingitem_UpdatesItem()
        {
            var existingItem = new TodoItem() { Id = 1, Title = "Expected Task" };

            _repositoryMock.Setup(repo => repo.UpdateAsync(existingItem));

            await _service.UpdateAsync(existingItem);

            _repositoryMock.Verify(repo => repo.UpdateAsync(existingItem), Times.Once);
        }

        [Test]
        public async Task DeleteAsync_ExistingItem_DeletesItem()
        {
            var existingItem = new TodoItem() { Id = 1, Title = "Existing Task." };

            _repositoryMock.Setup(repo => repo.DeleteAsync(existingItem));

            await _service.DeleteAsync(existingItem);

            _repositoryMock.Verify(repo => repo.DeleteAsync(existingItem), Times.Once);
        }

        [Test]
        public async Task DeleteAllAsync_CallsRepositoryDeleteAll()
        {
            _repositoryMock.Setup(repo => repo.DeleteAllAsync());

            await _service.DeleteAllAsync();

            _repositoryMock.Verify(repo => repo.DeleteAllAsync(), Times.Once);
        }
    }
}
