using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TodoLibrary;

namespace Zadatak2.Models.TodoViewModels
{
    public class TodoViewModel
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public DateTime? DateCompleted { get; set; }
        public DateTime DateCreated { get; set; }
        public List<TodoItemLabel> Labels { get; set; }
        public DateTime? DateDue { get; set; }

    }
}