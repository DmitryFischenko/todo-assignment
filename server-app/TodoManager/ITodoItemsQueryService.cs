using System.Collections.Generic;
using System.Threading.Tasks;
using TodoManager.Model;

namespace TodoManager
{
    public interface ITodoItemsQueryService
    {
        Task<TodoItem> GetByTitleAsync(string title);

        Task<IEnumerable<TodoItem>> GetActiveAsync();

        Task<IEnumerable<TodoItem>> GetCompletedAsync();

        Task<IEnumerable<TodoItem>> GetAllAsync();

        Task<TodoItem> GetByIdAsync(int id);
    }
}