using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using TodoManager.BusinessExceptions;
using TodoManager.DataAccess;
using TodoManager.DataAccess.DataModel;
using TodoManager.Model;

namespace TodoManager.Implementation
{
    public class TodoItemsManagementService: ITodoItemsManagementService
    {
        private const string TitleMustNotBeEmptyMessage = "Title must not be empty";
        
        private readonly ITodoItemsRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<TodoItemsManagementService> _logger;

        public TodoItemsManagementService(ITodoItemsRepository repository, IMapper mapper, ILogger<TodoItemsManagementService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<TodoItem> InsertAsync(TodoItem todoItem)
        {
            if (todoItem == null)
                throw new ArgumentNullException(nameof(todoItem));

            if (string.IsNullOrWhiteSpace(todoItem.Title))
                throw new ArgumentException(TitleMustNotBeEmptyMessage);

            var existingDto = await _repository.GetByTitleAsync(todoItem.Title);

            if (existingDto != null)
                throw new ItemAlreadyExistsException(todoItem.Title);

            var insertedDto = await _repository.InsertAsync(_mapper.Map<TodoItemDto>(todoItem));

            return _mapper.Map<TodoItem>(insertedDto);
        }

        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id));

            await _repository.DeleteAsync(id);
        }

        public async Task<TodoItem> UpdateAsync(TodoItem todoItem)
        {
            if (todoItem == null)
                throw new ArgumentNullException(nameof(todoItem));

            if (todoItem.Id <= 0)
                throw new ArgumentOutOfRangeException(nameof(todoItem.Id));

            if (string.IsNullOrWhiteSpace(todoItem.Title))
                throw new ArgumentException(TitleMustNotBeEmptyMessage, nameof(todoItem.Title));

            var itemWithSameName = await _repository.GetByTitleAsync(todoItem.Title);

            if (itemWithSameName != null && itemWithSameName.Id != todoItem.Id)
                throw new ItemAlreadyExistsException(todoItem.Title);
                
            var updatedDto = await _repository.UpdateAsync(_mapper.Map<TodoItemDto>(todoItem));

            return _mapper.Map<TodoItem>(updatedDto);
        }
    }
}