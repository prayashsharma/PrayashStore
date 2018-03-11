using PrayashStore.Models;
using PrayashStore.Services.Interfaces;
using System;
using System.Net;
using System.Web.Mvc;

namespace PrayashStore.Controllers
{
    [Authorize(Roles = "CanManageProducts,Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public ActionResult Index()
        {
            return View(_categoryService.GetAllCategories());
        }

        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category category)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(category);

                _categoryService.AddCategory(category);
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(category);
            }
        }

        public ActionResult Edit(int? id)
        {

            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var category = _categoryService.GetCategoryById(id.GetValueOrDefault());
            if (category == null)
                return HttpNotFound();

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category category)
        {
            if (!ModelState.IsValid)
                return View(category);

            _categoryService.EditCategory(category);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, message = "Category delete failed" });

            var category = _categoryService.GetCategoryById(id);
            _categoryService.RemoveCategory(category);

            return Json(new { success = true, message = $"Category: {category.Name} has been sucessfully deleted" });
        }

    }
}