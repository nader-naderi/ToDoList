using Microsoft.AspNetCore.Mvc;

using Moq;

using NUnit.Framework;

using System.Reflection;

using ToDoList.Controllers;
using ToDoList.Entities;
using ToDoList.Services;

namespace TodoListApp.UnitTest.ControllerTests
{
    [TestFixture]
    public class TestItemController
    {
        private ItemController _controller;
        private Mock<IService<TodoItem>> _serviceMock;

        [SetUp]
        public void Setup()
        {
            _serviceMock = new Mock<IService<TodoItem>>();
            _controller = new ItemController(_serviceMock.Object);
        }

        [Test]
        public async Task Post_ValidUser_ReturnsOk()
        {
            var validItem = new TodoItem { Id = 1, Title = "Test Subject 11" };

            _serviceMock.Setup(service => service.AddAsync(validItem))
                .ReturnsAsync(validItem);

            var result = await _controller.Post(validItem);

            Assert.IsInstanceOf<OkObjectResult>(result.Result);
        }

        [Test]
        public async Task Get_NonExistingItem_ReturnsNotFound()
        {
            var itemId = 900001;

            // What should i return when the getbyid method is called?
            _serviceMock.Setup(service => service.GetById(itemId))
                .ReturnsAsync(null as TodoItem);

            var result = await _controller.Get(itemId);

            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }


        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void GetItem_OnSuccess_ReturnsOK(int id)
        {
            // Arrange.
            var item = new TodoItem { Id = id, Title = "Test Subject." };
            _serviceMock.Setup(service => service.GetById(id)).ReturnsAsync(item);

            // Act
            var result = _controller.Get(id);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result.Result);
        }

        [Test]
        [TestCase(9100)]
        [TestCase(9200)]
        [TestCase(9300)]
        public void GetItem_OnNoUser_ReturnsNotFound(int id)
        {
            // Act
            var result = _controller.Get(id);

            // Assert
            Assert.That(result.Result.Result, Is.InstanceOf<NotFoundResult>(), "Expected an Not Found result.");
        }

        [Test]
        public void GetAllItem_OnSuccess_ReturnsOK()
        {
            // Act
            var result = _controller.GetAll();

            // Assert
            Assert.That(result.Result.Result, Is.InstanceOf<OkObjectResult>(), "Expected an OK result.");
        }
    }
}