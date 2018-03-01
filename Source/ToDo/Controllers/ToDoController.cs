using System;
using Microsoft.AspNetCore.Mvc;
using ToDo.Models.View;
using ToDo.Services;
using System.Threading.Tasks;

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
    }
}