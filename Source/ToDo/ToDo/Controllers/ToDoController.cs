using System;
using Microsoft.AspNetCore.Mvc;
using ToDo.Models.View;
using ToDo.Services;
using System.Threading.Tasks;
using ToDo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace ToDo.Controllers
{
    [Authorize]
    public class ToDoController : Controller
    {
        // Injeção de dependência
        private readonly IToDoItemService _todoItemsService;

        /* Aula 10, injeção de gerenciador de usuário (identity) */
        private readonly UserManager<ApplicationUser> _userManager;

        public ToDoController(IToDoItemService todoItemsService, UserManager<ApplicationUser> userManager)
        {
            _todoItemsService = todoItemsService;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index() 
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if(currentUser == null)
                return Challenge();

            // Acessar os dados
            var todoItems = await _todoItemsService.GetIncompleteItemsAsync(currentUser);
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
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
                return Unauthorized();

            var sucessful = await _todoItemsService
                .AddItemAsync(newToDoItem, currentUser);

            if (!sucessful)
                return BadRequest(new { Error = "Could not add Item" });

                return Ok();
        }

        public async Task<IActionResult> MarkDone(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest();

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
                return Unauthorized();

            var sucessful = await _todoItemsService
                .MarkDoneAsync(id, currentUser);

            return Ok();
        }
    }
}