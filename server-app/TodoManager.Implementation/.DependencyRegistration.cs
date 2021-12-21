using Microsoft.Extensions.DependencyInjection;

namespace TodoManager.Implementation
{
    public static class DependencyRegistration
    {
        public static void Register(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ITodoItemsManagementService, TodoItemsManagementService>();
            serviceCollection.AddTransient<ITodoItemsQueryService, TodoItemsQueryService>();
        }
    }
}