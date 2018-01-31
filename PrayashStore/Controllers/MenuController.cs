using PrayashStore.Models;
using PrayashStore.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
namespace PrayashStore.Controllers
{
    public class MenuController : Controller
    {
        private readonly ApplicationDbContext _context;
        public MenuController(ApplicationDbContext context)
        {
            _context = context;
        }

        [ChildActionOnly]
        public ActionResult Menu()
        {
            var categories = _context.Categories.ToList();

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