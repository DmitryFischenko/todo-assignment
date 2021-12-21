using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using TodoManager.BusinessExceptions;
using TodoManager.DataAccess;
using TodoManager.DataAccess.DataModel;
using TodoManager.Implementation.MapperProfiles;
using TodoManager.Model;

namespace TodoManager.Implementation.Tests
{
    [TestFixture]
    public class TodoItemsManagementServiceTests
    {
        private Mock<ILogger<TodoItemsManagementService>> _loggerMock;
        private Mock<ITodoItemsRepository> _todoItemsRepository;
        private IMapper _mapper;

        private TodoItemsManagementService _testee;
        
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile(new TodoItemProfile()))
                .CreateMapper();
        }

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<TodoItemsManagementService>>();
            _todoItemsRepository = new Mock<ITodoItemsRepository>();
            
            _testee = new TodoItemsManagementService(_todoItemsRepository.Object, _mapper, _loggerMock.Object);
        }

        [Test]
        public void Insert_ArgumentValidationTest()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => _testee.InsertAsync(null));

            Assert.ThrowsAsync<ArgumentException>(() => _testee.InsertAsync(new TodoItem()));
        }

        [Test]
        public void Insert_ShouldThrowWhenTitleExists()
        {
            const string title = "Todo1";

            _todoItemsRepository
                .Setup(r => r.GetByTitleAsync(title))
                .ReturnsAsync(new TodoItemDto()
                {
                    Title = title
                });

            Assert.ThrowsAsync<ItemAlreadyExistsException>(() => _testee.InsertAsync(new TodoItem() { Title = title }));
        }

        [Test]
        public async Task Insert_SuccessfulScenario()
        {
            const string title = "Todo1";
            const int insertedId = 1;
            
            TodoItemDto dtoToInsert = null;
            
            _todoItemsRepository
                .Setup(r => r.GetByTitleAsync(title))
                .ReturnsAsync((TodoItemDto) null);

            _todoItemsRepository
                .Setup(r => r.InsertAsync(It.IsAny<TodoItemDto>()))
                .Callback<TodoItemDto>(dto => dtoToInsert = dto)
                .ReturnsAsync(new TodoItemDto()
                {
                    Id = insertedId,
                    Title = title,
                    IsCompleted = true
                });

            var insertedModel = await _testee.InsertAsync(new TodoItem
            {
                Title = title,
                IsCompleted = true
            });

            Assert.That(dtoToInsert, Is.Not.Null);
            Assert.That(dtoToInsert.Title, Is.EqualTo(title));
            Assert.That(dtoToInsert.IsCompleted, Is.True);

            Assert.That(insertedModel, Is.Not.Null);
            Assert.That(insertedModel.Id, Is.EqualTo(insertedId));
            Assert.That(insertedModel.Title, Is.EqualTo(title));
            Assert.That(insertedModel.IsCompleted, Is.EqualTo(true));
        }
    }
}