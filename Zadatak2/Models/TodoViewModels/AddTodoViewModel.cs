using System;
using System.ComponentModel.DataAnnotations;

namespace Zadatak2.Models.TodoViewModels
{
    public class AddTodoViewModel
    {
        [Required]
        public string Text { get; set; }
        [Required]
        public DateTime? DateDue { get; set; }

        public string Message { get; set; }
        public string Labels { get; set; }

    }
}