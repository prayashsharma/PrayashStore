using PrayashStore.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace PrayashStore.Controllers
{
    [Authorize(Roles = "CanManageProducts,Admin")]
    public class CategoryController : Controller
    {

        private readonly ApplicationDbContext _context;
        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            var categories = _context.Categories.ToList();
            return View(categories);
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
            if (!ModelState.IsValid)
                return View(category);

            if (_context.Categories.Any(a => a.Name == category.Name))
            {
                ModelState.AddModelError("Name", "Cannot create category with duplicate name");
                return View(category);
            }

            _context.Categories.Add(category);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);


            var category = _context.Categories.Find(id);
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

            _context.Entry(category).State = EntityState.Modified;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, message = "Category delete failed" });

            var category = _context.Categories.Find(id);

            _context.Categories.Remove(category);
            _context.SaveChanges();

            return Json(new { success = true, message = $"Category: {category.Name} has been sucessfully deleted" });
        }

    }
}