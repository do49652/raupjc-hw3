using System.Data.Entity;
using TodoLibrary;

namespace Zadatak1
{
    public class TodoDbContext : DbContext
    {
        public TodoDbContext(string cnnstr) : base(cnnstr)
        {
        }

        public IDbSet<TodoItem> TodoItem { get; set; }
        public IDbSet<TodoItemLabel> TodoLabel { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TodoItem>().HasKey(i => i.Id);
            modelBuilder.Entity<TodoItem>().Property(i => i.Text).IsRequired();
            modelBuilder.Entity<TodoItem>().Property(i => i.DateCreated).IsRequired();
            modelBuilder.Entity<TodoItem>().Property(i => i.IsCompleted).IsRequired();
            modelBuilder.Entity<TodoItem>().Property(i => i.UserId).IsRequired();
            modelBuilder.Entity<TodoItem>().HasMany(i => i.Labels).WithMany(i => i.LabelTodoItems);

            modelBuilder.Entity<TodoItemLabel>().HasKey(i => i.Id);
            modelBuilder.Entity<TodoItemLabel>().Property(i => i.Value).IsRequired();
            modelBuilder.Entity<TodoItemLabel>().HasMany(i => i.LabelTodoItems).WithMany(i => i.Labels);
        }
    }
}