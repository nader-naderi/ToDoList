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
    }
}
