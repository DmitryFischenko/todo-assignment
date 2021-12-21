using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TodoManager.Web.Contracts;

namespace TodoManager.Web.Controllers
{
    [Route("api/todo-items")]
    public class TodoItemsController : Controller
    {
        private readonly ITodoItemsQueryService _todoItemsQueryService;
        private readonly ITodoItemsManagementService _todoItemsManagementService;
        private readonly IMapper _mapper;
        private readonly ILogger<TodoItemsController> _logger;

        public TodoItemsController(
            ITodoItemsQueryService todoItemsQueryService, 
            ITodoItemsManagementService todoItemsManagementService,
            IMapper mapper,
            ILogger<TodoItemsController> logger)
        {
            _todoItemsQueryService = todoItemsQueryService;
            _todoItemsManagementService = todoItemsManagementService;
            _mapper = mapper;
            _logger = logger;
        }
        
        [HttpGet]
        public async Task<IEnumerable<TodoItem>> Get(bool? isCompleted)
        {
            return isCompleted switch
            {
                false => (await _todoItemsQueryService.GetActiveAsync()).Select(item => _mapper.Map<TodoItem>(item)),
                true => (await _todoItemsQueryService.GetCompletedAsync()).Select(item => _mapper.Map<TodoItem>(item)),
                _ => (await _todoItemsQueryService.GetAllAsync()).Select(item => _mapper.Map<TodoItem>(item))
            };
        }

        [HttpPost]
        public async Task<TodoItem> Insert([FromBody]TodoItem todoItem)
        {
            if (todoItem == null)
                throw new BadHttpRequestException("Todo item content missing");

            if (string.IsNullOrWhiteSpace(todoItem.Title))
                throw new BadHttpRequestException("Title is mandatory");
            
            var createdItem = await _todoItemsManagementService.InsertAsync(_mapper.Map<Model.TodoItem>(todoItem));
            
            return _mapper.Map<Contracts.TodoItem>(createdItem);
        }

        [Route("{id}")]
        [HttpDelete]
        public async Task Delete(int id)
        {
            if (id <= 0)
                throw new BadHttpRequestException("Identifier is in wrong format");

            var existingItem = await _todoItemsQueryService.GetByIdAsync(id);

            if (existingItem == null)
                throw new BadHttpRequestException("Resource not found", (int)HttpStatusCode.NotFound);

            await _todoItemsManagementService.DeleteAsync(id);
            
            _logger.Log(LogLevel.Information, $"Item deleted. Id: {existingItem.Id}. Title: {existingItem.Title}");
        }

        [Route("{id}")]
        [HttpPut]
        public async Task<TodoItem> Update(int id, [FromBody]TodoItem todoItem)
        {
            if (id <= 0)
                throw new BadHttpRequestException("Identifier is in wrong format");

            if (todoItem == null)
                throw new BadHttpRequestException("Todo item content missing");

            if (string.IsNullOrEmpty(todoItem.Title))
                throw new BadHttpRequestException("Title is mandatory");
            
            var existingItem = await _todoItemsQueryService.GetByIdAsync(id);

            if (existingItem == null)
                throw new BadHttpRequestException("Resource not found", (int)HttpStatusCode.NotFound);
            
            todoItem.Id = id;

            var updatedModel = await _todoItemsManagementService.UpdateAsync(_mapper.Map<Model.TodoItem>(todoItem));

            return _mapper.Map<Contracts.TodoItem>(updatedModel);
        }
    }
}