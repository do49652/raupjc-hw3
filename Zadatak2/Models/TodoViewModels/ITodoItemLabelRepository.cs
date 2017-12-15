using System.Collections.Generic;
using System.Threading.Tasks;
using TodoLibrary;

namespace Zadatak2.Models.TodoViewModels
{
    public interface ITodoItemLabelRepository
    {
        Task<List<TodoItemLabel>> GetAllAsync();
        Task<TodoItemLabel> FindAsync(string value);
        Task Update(TodoItemLabel label, TodoItem todo);
    }
}