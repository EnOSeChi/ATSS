using ATSS.Application.Common.Mappings;
using ATSS.Domain.Entities;

namespace ATSS.Application.TodoLists.Queries.ExportTodos
{
    public class TodoItemRecord : IMapFrom<TodoItem>
    {
        public string Title { get; set; }

        public bool Done { get; set; }
    }
}
