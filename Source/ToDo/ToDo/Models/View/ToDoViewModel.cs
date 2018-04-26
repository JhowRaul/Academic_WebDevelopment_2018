using System.Collections.Generic;

namespace ToDo.Models.View
{
    public class ToDoViewModel
    {
        public IEnumerable<ToDoItem> Items { get; set; }
    }
}