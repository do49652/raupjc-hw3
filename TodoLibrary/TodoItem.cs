using System;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace TodoLibrary
{
    public class TodoItem
    {
        public TodoItem(string text)
        {
            Id = Guid.NewGuid();
            DateCreated = DateTime.UtcNow;
            Text = text;
        }

        public TodoItem(string text, Guid userId)
        {
            Id = Guid.NewGuid();
            Text = text;
            DateCreated = DateTime.UtcNow;
            UserId = userId;
            Labels = new List<TodoItemLabel>();
        }

        public TodoItem()
        {
            // entity framework needs this one
            // not for use :)
        }

        public Guid Id { get; set; }
        public string Text { get; set; }

        public bool IsCompleted
        {
            get => DateCompleted != null;
            set => DateCompleted = value ? DateTime.Now : (DateTime?)null;
        }

        public DateTime? DateCompleted { get; set; }
        public DateTime DateCreated { get; set; }

        /// <summary>
        ///     User id that owns this TodoItem
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        ///     /// List of labels associated with TodoItem
        /// </summary>
        public List<TodoItemLabel> Labels { get; set; }

        /// <summary>
        ///     Date due. If null, no date was set by the user
        /// </summary>
        public DateTime? DateDue { get; set; }

        public bool MarkAsCompleted()
        {
            if (IsCompleted) return false;
            DateCompleted = DateTime.Now;
            return true;
        }

        public override bool Equals(object obj)
        {
            return obj is TodoItem item &&
                   Id.Equals(item.Id);
        }

        public static bool operator ==(TodoItem t1, TodoItem t2)
        {
            return t1.Equals(t2);
        }

        public static bool operator !=(TodoItem t1, TodoItem t2)
        {
            return !t1.Equals(t2);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    /// <summary>
    ///     Label describing the TodoItem.
    ///     e.g. Critical, Personal, Work ...
    /// </summary>
    public class TodoItemLabel
    {
        public TodoItemLabel()
        {
        }

        public TodoItemLabel(string value)
        {
            Id = Guid.NewGuid();
            Value = value;
            LabelTodoItems = new List<TodoItem>();
        }

        public Guid Id { get; set; }
        public string Value { get; set; }

        /// <summary>
        ///     All TodoItems that are associated with this label
        /// </summary>
        public List<TodoItem> LabelTodoItems { get; set; }
    }
}