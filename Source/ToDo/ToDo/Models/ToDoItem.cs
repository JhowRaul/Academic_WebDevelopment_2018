using System;
using System.Linq;

namespace ToDo.Models
{
    public class ToDoItem
    {
        public Guid Id { get; set; }
        public bool IsDone { get; set; }
        public string Title { get; set; }
        public DateTimeOffset? DueAt { get; set; }
        public DateTimeOffset UpUntil { get; set; }
        public string OwnerId { get; set; }
        
    }
}