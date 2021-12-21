using AutoMapper;

namespace TodoManager.Web.MapperProfiles
{
    public class TodoItemProfile: Profile
    {
        public TodoItemProfile()
        {
            CreateMap<Contracts.TodoItem, Model.TodoItem>().ReverseMap();
        }
    }
}