using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TodoLibrary;
using Zadatak2.Models;
using Zadatak2.Models.TodoViewModels;
using IndexViewModel = Zadatak2.Models.TodoViewModels.IndexViewModel;

namespace Zadatak2.Controllers
{
    [Authorize]
    public class TodoController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITodoRepository _repository;

        public TodoController(UserManager<ApplicationUser> userManager, ITodoRepository repository)
        {
            _userManager = userManager;
            _repository = repository;
        }

        public async Task<ViewResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var todoitems = _repository.GetActive(Guid.Parse(currentUser.Id));

            var indexViewModel = new IndexViewModel()
            {
                TodoViewModels = todoitems.Select(i => new TodoViewModel()
                {
                    Id = i.Id.ToString(),
                    Text = i.Text,
                    DateCreated = i.DateCreated,
                    DateDue = i.DateDue,
                    DateCompleted = i.DateCompleted,
                    Labels = i.Labels
                }).ToList()
            };

            return View(indexViewModel);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(TodoViewModel todoViewModel)
        {
            if (todoViewModel.Text.Length == 0)
            {
                return View(todoViewModel);
            }

            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var todoItem = new TodoItem(todoViewModel.Text, Guid.Parse(currentUser.Id))
            {
                DateDue = todoViewModel.DateDue
            };
            _repository.Add(todoItem);

            return RedirectToAction("Index");
        }

        public async Task<ViewResult> Completed()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var todoitems = _repository.GetCompleted(Guid.Parse(currentUser.Id));

            var completedViewModel = new CompletedViewModel()
            {
                TodoViewModels = todoitems.Select(i => new TodoViewModel()
                {
                    Id = i.Id.ToString(),
                    Text = i.Text,
                    DateCreated = i.DateCreated,
                    DateDue = i.DateDue,
                    DateCompleted = i.DateCompleted,
                    Labels = i.Labels
                }).ToList()
            };

            return View(completedViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Completed(TodoViewModel todoViewModel)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            _repository.MarkAsCompleted(Guid.Parse(todoViewModel.Id), Guid.Parse(currentUser.Id));

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> NotCompleted(TodoViewModel todoViewModel)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var todo = _repository.Get(Guid.Parse(todoViewModel.Id), Guid.Parse(currentUser.Id));
            todo.DateCompleted = null;
            _repository.Update(todo, Guid.Parse(currentUser.Id));

            return RedirectToAction("Completed");
        }

    }
}