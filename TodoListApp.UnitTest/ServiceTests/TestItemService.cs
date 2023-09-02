using Moq;

using NUnit.Framework;

using ToDoList.Entities;
using ToDoList.Repository;
using ToDoList.Services;

namespace TodoListApp.UnitTest.ServiceTests
{
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
    }
}
