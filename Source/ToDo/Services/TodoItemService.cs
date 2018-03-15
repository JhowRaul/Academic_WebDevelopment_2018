using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDo.Data;
using ToDo.Models;

// Aula 5
namespace ToDo.Services
{
    public class ToDoItemService : IToDoItemService
    {
        private readonly ApplicationDbContext _context;

        public ToDoItemService (ApplicationDbContext context) {
            _context = context;
        }
        
        // Função assincrona tem q estar especificado async
        public async Task<IEnumerable<ToDoItem>> GetIncompleteItemsAsync() {
            var items = await _context.Items
                .Where(x => x.IsDone == false)
                .ToArrayAsync();

            return items;
        }

        public async Task<bool> AddItemAsync (NewToDoItem newToDoItem) {
            var entity = new ToDoItem
            {
                Id = Guid.NewGuid(),
                IsDone = false,
                Title = newToDoItem.Title,
                DueAt = DateTimeOffset.Now.AddDays(3)
            };
            _context.Items.Add(entity);
            var saveResult = await _context.SaveChangesAsync();
            return saveResult == 1;
        }
    }
}