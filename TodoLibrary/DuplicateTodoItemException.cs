using System;

namespace TodoLibrary
{
    public class DuplicateTodoItemException : Exception
    {
        public DuplicateTodoItemException(string message) : base(message)
        {
        }
    }
}