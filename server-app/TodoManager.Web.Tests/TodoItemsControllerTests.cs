using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using TodoManager.Web.Contracts;
using TodoManager.Web.Controllers;
using TodoManager.Web.MapperProfiles;

namespace TodoManager.Web.Tests
{
    [TestFixture]
    public class TodoItemsControllerTests
    {
        private TodoItemsController _testee;
        private Mock<ITodoItemsManagementService> _todoItemsManagementServiceMock;
        private Mock<ITodoItemsQueryService> _todoItemsQueryServiceMock;
        private Mock<ILogger<TodoItemsController>> _loggerMock;
        private IMapper _mapper;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile(new TodoItemProfile()))
                .CreateMapper();
        }
        
        [SetUp]
        public void SetUp()
        {
            _todoItemsManagementServiceMock = new Mock<ITodoItemsManagementService>();
            _todoItemsQueryServiceMock = new Mock<ITodoItemsQueryService>();
            _loggerMock = new Mock<ILogger<TodoItemsController>>();
            
            _testee = new TodoItemsController(
                _todoItemsQueryServiceMock.Object, 
                _todoItemsManagementServiceMock.Object,
                _mapper,
                _loggerMock.Object);
        }

        [Test]
        public void Insert_ShouldThrowBadRequestWhenContentIsMissing()
        {
            Assert.ThrowsAsync<BadHttpRequestException>(() => _testee.Insert(null));
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase("   ")]
        public void Insert_ShouldThrowBadRequestWhenTitleIsMissing(string title)
        {
            Assert.ThrowsAsync<BadHttpRequestException>(() => _testee.Insert(new TodoItem()
            {
                Title = title
            }));
        }

        [Test]
        public async Task Insert_ShouldSucceedWhenRequestIsCorrect()
        {
            const string title = "Todo1";

            var requestContract = new TodoItem
            {
                Title = title,
                IsCompleted = true
            };

            _todoItemsQueryServiceMock
                .Setup(s => s.GetByTitleAsync(title))
                .ReturnsAsync(() => null);

            Model.TodoItem modelToInsert = null;

            var insertedModel = new Model.TodoItem
            {
                Id = 1,
                Title = title,
                IsCompleted = true
            };

            _todoItemsManagementServiceMock
                .Setup(s => s.InsertAsync(It.IsAny<Model.TodoItem>()))
                .Callback<Model.TodoItem>(item => modelToInsert = item)
                .ReturnsAsync(insertedModel);
            
            var responseContract = await _testee.Insert(requestContract);
            
            _todoItemsManagementServiceMock.Verify(s => s.InsertAsync(It.IsAny<Model.TodoItem>()), Times.Once);

            Assert.That(modelToInsert, Is.Not.Null);
            Assert.That(modelToInsert.Title, Is.EqualTo(requestContract.Title));
            Assert.That(modelToInsert.IsCompleted, Is.EqualTo(requestContract.IsCompleted));
            
            Assert.That(responseContract, Is.Not.Null);
            Assert.That(responseContract.Id, Is.EqualTo(insertedModel.Id));
            Assert.That(responseContract.Title, Is.EqualTo(insertedModel.Title));
            Assert.That(responseContract.IsCompleted, Is.EqualTo(insertedModel.IsCompleted));
        }

        [Test]
        public async Task Get_CallsProperServiceMethod()
        {
            await _testee.Get(false);
            
            _todoItemsQueryServiceMock.Verify(s => s.GetActiveAsync(), Times.Once);
            _todoItemsQueryServiceMock.Reset();

            await _testee.Get(true);
            
            _todoItemsQueryServiceMock.Verify(s => s.GetCompletedAsync(), Times.Once);
            _todoItemsQueryServiceMock.Reset();

            await _testee.Get(null);
            
            _todoItemsQueryServiceMock.Verify(s => s.GetAllAsync(), Times.Once);
        }

        [Test]
        public async Task Get_ConvertsModelToContractProperly()
        {
            var firstItemModel = new Model.TodoItem()
            {
                Id = 1, Title = "todo1", IsCompleted = true
            };

            var secondItemModel = new Model.TodoItem()
            {
                Id = 2, Title = "todo2", IsCompleted = false
            };

            _todoItemsQueryServiceMock
                .Setup(s => s.GetAllAsync())
                .ReturnsAsync(new List<Model.TodoItem>
                {
                    firstItemModel,
                    secondItemModel
                });

            var response = (await _testee.Get(null)).ToArray();
            
            Assert.That(response, Is.Not.Null);
            Assert.That(response.Count, Is.EqualTo(2));

            var firstItemContract = response.FirstOrDefault(i => i.Id == firstItemModel.Id);
            
            Assert.That(firstItemContract, Is.Not.Null);
            Assert.That(firstItemContract.Title, Is.EqualTo(firstItemModel.Title));
            Assert.That(firstItemContract.IsCompleted, Is.EqualTo(firstItemModel.IsCompleted));

            var secondItemContract = response.FirstOrDefault(i => i.Id == secondItemModel.Id);
            Assert.That(secondItemContract, Is.Not.Null);
            Assert.That(secondItemContract.Title, Is.EqualTo(secondItemModel.Title));
            Assert.That(secondItemContract.IsCompleted, Is.EqualTo(secondItemModel.IsCompleted));
        }
    }
}