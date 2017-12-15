using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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
        private readonly ITodoItemLabelRepository _labelRepository;

        public TodoController(UserManager<ApplicationUser> userManager, ITodoRepository repository, ITodoItemLabelRepository labelRepository)
        {
            _userManager = userManager;
            _repository = repository;
            _labelRepository = labelRepository;
        }

        public async Task<ViewResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var todoitems = _repository.GetActive(Guid.Parse(currentUser.Id)).OrderBy(t => t.DateDue);

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

        public IActionResult Add(AddTodoViewModel todoViewModel)
        {
            if (todoViewModel == null)
                todoViewModel = new AddTodoViewModel();
            return View(todoViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddNew(AddTodoViewModel todoViewModel)
        {
            if (!ModelState.IsValid)
            {
                todoViewModel.Message = "Text field must not be empty.";
                return RedirectToAction("Add", todoViewModel);
            }

            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var todoItem = new TodoItem(todoViewModel.Text, Guid.Parse(currentUser.Id))
            {
                DateDue = todoViewModel.DateDue
            };
            _repository.Update(todoItem, Guid.Parse(currentUser.Id));

            if (todoViewModel.Labels == null || todoViewModel.Labels.Trim().Length <= 0)
                return RedirectToAction("Index");

            var labels = new SortedSet<string>(todoViewModel.Labels.ToLower().Replace(",", " ").Replace("  ", " ").Trim().Split(' ').ToList());
            foreach (var l in labels)
            {
                var label = new TodoItemLabel(l);
                await _labelRepository.Update(label, todoItem);
            }

            return RedirectToAction("Index");
        }

        public async Task<ViewResult> Completed()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var todoitems = _repository.GetCompleted(Guid.Parse(currentUser.Id)).OrderBy(t => t.DateDue);

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