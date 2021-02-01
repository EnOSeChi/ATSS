using RecruitmentTask.Application.Common.Mappings;
using RecruitmentTask.Domain.Entities;

namespace RecruitmentTask.Application.TodoLists.Queries.ExportTodos
{
    public class TodoItemRecord : IMapFrom<TodoItem>
    {
        public string Title { get; set; }

        public bool Done { get; set; }
    }
}
