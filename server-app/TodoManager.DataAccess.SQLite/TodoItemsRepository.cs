using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TodoManager.DataAccess.DataModel;
using TodoManager.DataAccess.SQLite.Entities;

namespace TodoManager.DataAccess.SQLite
{
    internal class TodoItemsRepository: ITodoItemsRepository
    {
        private readonly TodoDbContext _dbContext;

        public TodoItemsRepository(TodoDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task<TodoItemDto> InsertAsync(TodoItemDto todoItemDto)
        {
            if (todoItemDto == null)
                throw new ArgumentNullException(nameof(todoItemDto));

            if (string.IsNullOrEmpty(todoItemDto.Title))
                throw new ArgumentException(nameof(todoItemDto.Title));

            var result = await _dbContext.TodoItems.AddAsync(new TodoItem()
            {
                Title = todoItemDto.Title,
                IsCompleted = todoItemDto.IsCompleted
            });

            await _dbContext.SaveChangesAsync();

            return new TodoItemDto()
            {
                Id = result.Entity.Id,
                Title = result.Entity.Title,
                IsCompleted = result.Entity.IsCompleted
            };
        }

        public async Task<TodoItemDto> UpdateAsync(TodoItemDto todoItemDto)
        {
            if (todoItemDto == null)
                throw new ArgumentNullException(nameof(todoItemDto));

            if (string.IsNullOrEmpty(todoItemDto.Title))
                throw new ArgumentException(nameof(todoItemDto.Title));

            var existingEntity = await _dbContext.TodoItems.FindAsync(todoItemDto.Id);

            if (existingEntity == null)
                throw new InvalidOperationException($"Item with id '{todoItemDto.Id} does not exist");

            existingEntity.Title = todoItemDto.Title;
            existingEntity.IsCompleted = todoItemDto.IsCompleted;

            await _dbContext.SaveChangesAsync();

            return new TodoItemDto
            {
                Id = existingEntity.Id,
                Title = existingEntity.Title,
                IsCompleted = existingEntity.IsCompleted
            };
        }

        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id));

            _dbContext.TodoItems.Remove(new TodoItem() { Id = id });

            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<TodoItemDto>> GetAsync(bool? isCompleted)
        {
            var query = _dbContext.TodoItems.AsNoTracking();

            if (isCompleted != null)
                query = query.Where(entity => entity.IsCompleted == isCompleted);
            
            return await query
                .Select((entity => new TodoItemDto
                    {
                        Id = entity.Id,
                        Title = entity.Title,
                        IsCompleted = entity.IsCompleted
                    }))
                .ToListAsync();
        }

        public async Task<TodoItemDto> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id));
            
            var result = await _dbContext.TodoItems
                .FindAsync(id);

            if (result == null)
                return null;
            
            _dbContext.Entry(result).State = EntityState.Detached;

            return new TodoItemDto
            {
                Id = result.Id,
                Title = result.Title,
                IsCompleted = result.IsCompleted
            };
        }

        public async Task<TodoItemDto> GetByTitleAsync(string title)
        {
            if (string.IsNullOrEmpty(title))
                throw new ArgumentException(nameof(title));
            
            var foundEntity = await _dbContext.TodoItems
                .AsNoTracking()
                .Where(item => item.Title.ToLower() == title.ToLower())
                .FirstOrDefaultAsync();

            if (foundEntity == null)
                return null;

            return new TodoItemDto()
            {
                Id = foundEntity.Id,
                Title = foundEntity.Title,
                IsCompleted = foundEntity.IsCompleted
            };
        }
    }
}