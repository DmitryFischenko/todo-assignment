using Microsoft.Extensions.DependencyInjection;

namespace TodoManager.DataAccess.SQLite
{
    public static class DependencyRegistration
    {
        public static void Register(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ITodoItemsRepository, TodoItemsRepository>();
            serviceCollection.AddDbContext<TodoDbContext>();
        }
    }
}