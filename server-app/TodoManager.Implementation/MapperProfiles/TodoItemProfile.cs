using AutoMapper;

namespace TodoManager.Implementation.MapperProfiles
{
    public class TodoItemProfile: Profile
    {
        public TodoItemProfile()
        {
            CreateMap<Model.TodoItem, DataAccess.DataModel.TodoItemDto>().ReverseMap();
        }
    }
}