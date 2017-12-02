using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using TodoLibrary;

namespace Zadatak1
{
    public class TodoSqlRepository : ITodoRepository
    {
        private const string ConnectionString =
                "Server=(localdb)\\mssqllocaldb;Database=TodoDbContext;Trusted_Connection=True;MultipleActiveResultSets=true"
            ;

        public TodoItem Get(Guid todoId, Guid userId)
        {
            using (var db = new TodoDbContext(ConnectionString))
            {
                var todoItem = db.TodoItem.Include(k => k.Labels).FirstOrDefault(s => s.Id.Equals(todoId));

                if (todoItem == null)
                    return null;

                if (!todoItem.UserId.Equals(userId))
                    throw new TodoAccessDeniedException();

                return todoItem;
            }
        }

        public void Add(TodoItem todoItem)
        {
            using (var db = new TodoDbContext(ConnectionString))
            {
                if (db.TodoItem.Contains(todoItem))
                    throw new DuplicateTodoItemException("duplicate id: " +
                                                         db.TodoItem.FirstOrDefault(s => s.Equals(todoItem)));

                db.TodoItem.Add(todoItem);
                db.SaveChanges();
            }
        }

        public bool Remove(Guid todoId, Guid userId)
        {
            var todoItem = Get(todoId, userId);

            if (todoItem == null)
                return false;

            using (var db = new TodoDbContext(ConnectionString))
            {
                db.TodoItem.Remove(todoItem);
                db.SaveChanges();
            }
            return true;
        }

        public void Update(TodoItem todoItem, Guid userId)
        {
            var oItem = Get(todoItem.Id, userId);

            if (oItem == null)
            {
                Add(todoItem);
                return;
            }

            using (var db = new TodoDbContext(ConnectionString))
            {
                db.Entry(todoItem).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public bool MarkAsCompleted(Guid todoId, Guid userId)
        {
            var todoItem = Get(todoId, userId);

            if (todoItem == null || !todoItem.MarkAsCompleted())
                return false;

            using (var db = new TodoDbContext(ConnectionString))
            {
                db.Entry(todoItem).State = EntityState.Modified;
                db.SaveChanges();
            }
            return true;
        }

        public List<TodoItem> GetAll(Guid userId)
        {
            using (var db = new TodoDbContext(ConnectionString))
            {
                return db.TodoItem.Include(k => k.Labels).OrderByDescending(s => s.DateCreated).ToList();
            }
        }

        public List<TodoItem> GetActive(Guid userId)
        {
            using (var db = new TodoDbContext(ConnectionString))
            {
                return db.TodoItem.Include(k => k.Labels).Where(s => !s.IsCompleted).ToList();
            }
        }

        public List<TodoItem> GetCompleted(Guid userId)
        {
            using (var db = new TodoDbContext(ConnectionString))
            {
                return db.TodoItem.Include(k => k.Labels).Where(s => s.IsCompleted).ToList();
            }
        }

        public List<TodoItem> GetFiltered(Func<TodoItem, bool> filterFunction, Guid userId)
        {
            using (var db = new TodoDbContext(ConnectionString))
            {
                return db.TodoItem.Include(k => k.Labels).AsEnumerable().Where(filterFunction).ToList();
            }
        }
    }
}