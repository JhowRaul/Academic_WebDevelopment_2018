using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDo.Models;

namespace ToDo.Services
{
    public interface IToDoItemService
    {
        // Criação de interface
        Task<IEnumerable<ToDoItem>> GetIncompleteItemsAsync();
        Task<bool> AddItemAsync (NewToDoItem newToDoItem);
    }
}