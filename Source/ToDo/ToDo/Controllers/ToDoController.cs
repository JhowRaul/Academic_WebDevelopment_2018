using System;
using Microsoft.AspNetCore.Mvc;
using ToDo.Models.View;
using ToDo.Services;
using System.Threading.Tasks;
using ToDo.Models;

namespace ToDo.Controllers
{
    public class ToDoController : Controller
    {
        // Injeção de dependência
        private readonly IToDoItemService _todoItemsService;

        public ToDoController(IToDoItemService todoItemsService)
        {
            _todoItemsService = todoItemsService;
        }
        public async Task<IActionResult> Index() 
        {
            // Acessar os dados
            var todoItems = await _todoItemsService.GetIncompleteItemsAsync();
            // Montar uma Model
            var viewModel = new ToDoViewModel
            {
                Items = todoItems
            };
            // Retornar View
            return View(viewModel);
        }

        public async Task<IActionResult> AddItem (NewToDoItem newToDoItem)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var sucessful = await _todoItemsService
                .AddItemAsync(newToDoItem);

            if (!sucessful)
                return BadRequest(new { Error = "Could not add Item" });

                return Ok();
        }

        public async Task<IActionResult> MarkDone(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest();

            var sucessful = await _todoItemsService
                .MarkDoneAsync(id);

            return Ok();
        }
    }
}