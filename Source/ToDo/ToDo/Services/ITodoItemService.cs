using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDo.Models;

namespace ToDo.Services
{
    public interface IToDoItemService
    {
        // Criação de interface
        Task<IEnumerable<ToDoItem>> GetIncompleteItemsAsync(ApplicationUser currentUser);
        Task<bool> AddItemAsync (NewToDoItem newToDoItem, ApplicationUser currentUser);
        Task<bool> MarkDoneAsync (Guid id, ApplicationUser currentUser);
    }
}