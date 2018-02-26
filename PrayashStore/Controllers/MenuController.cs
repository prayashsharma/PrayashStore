using PrayashStore.Models;
using PrayashStore.Services.Interfaces;
using PrayashStore.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
namespace PrayashStore.Controllers
{
    public class MenuController : Controller
    {
        private readonly ICategoryService _categoryService;
        public MenuController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [ChildActionOnly]
        public ActionResult Menu()
        {
            var categories = _categoryService.GetAllCategories().ToList();

            List<MenuViewModel> menuViewModels = new List<MenuViewModel>();

            var genders = Enum.GetNames(typeof(Gender));

            foreach (var g in genders)
            {
                menuViewModels.Add(new MenuViewModel(g, categories));
            }

            ViewData["Menu"] = menuViewModels;

            return PartialView("_Menu");
        }
    }
}