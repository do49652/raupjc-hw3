using System;
using System.Collections.Generic;
using System.Linq;
using GenericListLibrary;

namespace TodoLibrary
{
    /// <summary>
    ///     Class that encapsulates all the logic for accessing TodoTtems.
    /// </summary>
    public class TodoRepository : ITodoRepository
    {
        /// <summary>
        ///     Repository does not fetch todoItems from the actual database,
        ///     it uses in memory storage for this excersise.
        /// </summary>
        private readonly IGenericList<TodoItem> _inMemoryTodoDatabase;

        public TodoRepository(IGenericList<TodoItem> initialDbState = null)
        {
            _inMemoryTodoDatabase = initialDbState ?? new GenericList<TodoItem>();
        }

        public TodoItem Get(Guid todoId)
        {
            return _inMemoryTodoDatabase.FirstOrDefault(s => s.Id == todoId);
        }

        public TodoItem Add(TodoItem todoItem)
        {
            if (_inMemoryTodoDatabase.Contains(todoItem))
                throw new DuplicateTodoItemException("duplicate id: " +
                                                     _inMemoryTodoDatabase.FirstOrDefault(s => s.Equals(todoItem)));

            _inMemoryTodoDatabase.Add(todoItem);
            return todoItem;
        }

        public bool Remove(Guid todoId)
        {
            return _inMemoryTodoDatabase.Remove(Get(todoId));
        }

        public TodoItem Update(TodoItem todoItem)
        {
            var oItem = Get(todoItem.Id);

            if (oItem == null)
                return Add(todoItem);

            oItem.Text = todoItem.Text;
            oItem.DateCompleted = todoItem.DateCompleted;
            oItem.DateCreated = todoItem.DateCreated;

            if (todoItem.IsCompleted)
                oItem.MarkAsCompleted();

            return oItem;
        }

        public bool MarkAsCompleted(Guid todoId)
        {
            return Get(todoId) != null && Get(todoId).MarkAsCompleted();
        }

        public List<TodoItem> GetAll()
        {
            return _inMemoryTodoDatabase.OrderByDescending(s => s.DateCreated).ToList();
        }

        public List<TodoItem> GetActive()
        {
            return _inMemoryTodoDatabase.Where(s => !s.IsCompleted).ToList();
        }

        public List<TodoItem> GetCompleted()
        {
            return _inMemoryTodoDatabase.Where(s => s.IsCompleted).ToList();
        }

        public List<TodoItem> GetFiltered(Func<TodoItem, bool> filterFunction)
        {
            return _inMemoryTodoDatabase.Where(filterFunction).ToList();
        }
    }
}