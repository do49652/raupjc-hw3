using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using TodoLibrary;
using Zadatak1;

namespace Zadatak2.Models.TodoViewModels
{
    public class TodoItemLabelRepository : ITodoItemLabelRepository
    {
        private static string _connectionString = "Server=(localdb)\\mssqllocaldb;Database=TodoDbContext;Trusted_Connection=True;MultipleActiveResultSets=true";

        public TodoItemLabelRepository(string cnnstr)
        {
            _connectionString = cnnstr;
        }

        public async Task<List<TodoItemLabel>> GetAllAsync()
        {
            using (var db = new TodoDbContext(_connectionString))
            {
                return db.TodoLabel.Include(l => l.LabelTodoItems).ToList();
            }
        }

        public async Task<TodoItemLabel> FindAsync(string value)
        {
            return (await GetAllAsync()).FirstOrDefault(l => l.Value.Equals(value));
        }

        public async Task Update(TodoItemLabel label, TodoItem todo)
        {
            using (var db = new TodoDbContext(_connectionString))
            {
                var l = await db.TodoLabel.SingleOrDefaultAsync(t => t.Value.Equals(label.Value));
                var i = await db.TodoItem.Include(t => t.Labels).SingleAsync(t => t.Id.Equals(todo.Id));

                if (l == null)
                    l = label;

                i.Labels.Add(l);

                db.Entry(i).State = EntityState.Modified;
                db.SaveChanges();
            }
        }
    }
}