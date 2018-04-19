using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDo.Data;
using ToDo.Models;
using Microsoft.AspNetCore.Identity;

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
        public async Task<IEnumerable<ToDoItem>> GetIncompleteItemsAsync(ApplicationUser currentUser) {
            var items = await _context.Items
                .Where(x => x.IsDone == false)
                .ToArrayAsync();

            return items;
        }

        public async Task<bool> AddItemAsync (NewToDoItem newToDoItem, ApplicationUser currentUser)
        {
            var entity = new ToDoItem
            {
                Id = Guid.NewGuid(),
                IsDone = false,
                Title = newToDoItem.Title,
                DueAt = newToDoItem.DueAt,
                OwnerId = currentUser.Id
            };

            _context.Items.Add(entity);
            var saveResult = await _context.SaveChangesAsync();
            return saveResult == 1;
        }

        public async Task<bool> MarkDoneAsync(Guid id, ApplicationUser currentUser)
        {
            var item = await _context.Items
                .Where(x => x.Id == id)
                .SingleOrDefaultAsync();

            if (item == null)
                return false;

            item.IsDone = true;

            var saveResult = await _context
                .SaveChangesAsync();

            // One entity should
            // have been updated
            return saveResult == 1;
        }
    }
}