using ATSS.Application.TodoLists.Queries.ExportTodos;
using System.Collections.Generic;

namespace ATSS.Application.Common.Interfaces
{
    public interface ICsvFileBuilder
    {
        byte[] BuildTodoItemsFile(IEnumerable<TodoItemRecord> records);
    }
}
