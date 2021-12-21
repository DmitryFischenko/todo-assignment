using System.Collections.Generic;
using System.Threading.Tasks;
using TodoManager.DataAccess.DataModel;

namespace TodoManager.DataAccess
{
    public interface ITodoItemsRepository
    {
        Task<TodoItemDto> InsertAsync(TodoItemDto todoItemDto);

        Task<TodoItemDto> UpdateAsync(TodoItemDto todoItemDto);

        Task DeleteAsync(int id);

        Task<IEnumerable<TodoItemDto>> GetAsync(bool? isCompleted);

        Task<TodoItemDto> GetByIdAsync(int id);

        Task<TodoItemDto> GetByTitleAsync(string title);
    }
}