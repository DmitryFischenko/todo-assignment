using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using TodoManager.DataAccess;
using TodoManager.DataAccess.DataModel;
using TodoManager.Model;

namespace TodoManager.Implementation
{
    internal class TodoItemsQueryService: ITodoItemsQueryService
    {
        private readonly ITodoItemsRepository _repository;
        private readonly IMapper _mapper;

        public TodoItemsQueryService(ITodoItemsRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        
        public async Task<TodoItem> GetByTitleAsync(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title must not be empty", nameof(title));
            
            var todoItemDto = await _repository.GetByTitleAsync(title);
            return _mapper.Map<Model.TodoItem>(todoItemDto);
        }

        public async Task<IEnumerable<TodoItem>> GetActiveAsync()
        {
            return (await _repository.GetAsync(false)).Select(dto => _mapper.Map<TodoItem>(dto));
        }

        public async Task<IEnumerable<TodoItem>> GetCompletedAsync()
        {
            return (await _repository.GetAsync(true)).Select(dto => _mapper.Map<TodoItem>(dto));
        }

        public async Task<IEnumerable<TodoItem>> GetAllAsync()
        {
            return (await _repository.GetAsync(null)).Select(dto => _mapper.Map<TodoItem>(dto));
        }

        public async Task<TodoItem> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), "Id must be greater then 0");
            
            var dto = await _repository.GetByIdAsync(id);
            return _mapper.Map<TodoItem>(dto);
        }
    }
}