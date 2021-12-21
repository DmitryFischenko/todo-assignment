using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using TodoManager.DataAccess.DataModel;

namespace TodoManager.DataAccess.SQLite.IntegrationTests
{
    [TestFixture]
    public class TodoItemsRepositoryTests
    {
        private TodoItemsRepository _testee;
        private TodoDbContext _dbContext;
        
        [OneTimeSetUp]
        public void OnTimeSetUp()
        {
            var configMock = new Mock<IConfiguration>();
            configMock.SetupGet(c => c["SQLite:Connection"]).Returns("Data Source=./TodoManagerTest.db");
            _dbContext = new TodoDbContext(configMock.Object);
            _dbContext.Database.EnsureCreated();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _dbContext.Database.EnsureDeleted();
        }

        [SetUp]
        public void Setup()
        {
            _testee = new TodoItemsRepository(_dbContext);
        }

        [Test]
        public void InsertAsync_ArgumentValidation()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => _testee.InsertAsync(null));
            Assert.ThrowsAsync<ArgumentException>(() => _testee.InsertAsync(new TodoItemDto()));
        }

        [Test]
        public async Task InsertAsync_InsertItemToDbSuccessfully()
        {
            var dto = new TodoItemDto()
            {
                Title = "ToDo1",
                IsCompleted = true
            };

            var result = await _testee.InsertAsync(dto);
            
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.GreaterThan(0));
            Assert.That(result.Title, Is.EqualTo(dto.Title));
            Assert.That(result.IsCompleted, Is.EqualTo(dto.IsCompleted));

            var loadedById = await _testee.GetByIdAsync(result.Id);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.GreaterThan(0));
            Assert.That(result.Title, Is.EqualTo(dto.Title));
            Assert.That(result.IsCompleted, Is.EqualTo(dto.IsCompleted));

        }
    }
}