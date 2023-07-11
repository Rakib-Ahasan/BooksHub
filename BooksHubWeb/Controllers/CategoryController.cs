using BooksHub.DataAccess.Repository.IRepository;
using BooksHubWeb.Data;
using BooksHubWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace BooksHubWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _db;

        public CategoryController(ICategoryRepository db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<Category> categoryObj = _db.GetAll();
            return View(categoryObj);
        }

        //Get
        public IActionResult Create()
        {
           
            return View();
        }

        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            if(obj.Name == obj.DisplayOrder.ToString()) 
            {
                //ModelState.AddModelError("CustomError", "Display Order Should Not Be The Exact Name");
                ModelState.AddModelError("Name", "Display Order Should Not Be The Exact Name");
            }
            if (ModelState.IsValid) 
            {
                _db.Add(obj);
                _db.Save();
                TempData["success"] = "Category created successfully.";
                return RedirectToAction("Index");
            }
            return View(obj); 
        }

        //Get
        public IActionResult Edit(int? id)
        {
            if(id== null || id == 0 )
            {
                return NotFound();
            }

            var objFormDb = _db.GetFirstOrDefault(u=>u.Id == id);
            if(objFormDb == null )
            {
                return NotFound();
            }

            return View(objFormDb);
        }

        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                //ModelState.AddModelError("CustomError", "Display Order Should Not Be The Exact Name");
                ModelState.AddModelError("Name", "Display Order Should Not Be The Exact Name");
            }
            if (ModelState.IsValid)
            {
                _db.Update(obj);
                _db.Save();
                TempData["success"] = "Category updated successfully.";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        //Get
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var objFormDb = _db.GetFirstOrDefault(u=>u.Id == id);
            if (objFormDb == null)
            {
                return NotFound();
            }

            return View(objFormDb);
        }

        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int id)
        {
            var obj = _db.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _db.Remove(obj);
            _db.Save();
            TempData["success"] = "Category deleted successfully.";
            return RedirectToAction("Index");
        }
    }
}
