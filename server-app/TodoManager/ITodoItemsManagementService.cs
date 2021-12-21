using System.Threading.Tasks;
using TodoManager.Model;

namespace TodoManager
{
    public interface ITodoItemsManagementService
    {
        Task<TodoItem> InsertAsync(TodoItem todoItem);

        Task DeleteAsync(int id);

        Task<TodoItem> UpdateAsync(TodoItem todoItem);
    }
}