using BooksHub.DataAccess.Repository.IRepository;
using BooksHubWeb.Data;
using BooksHubWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace BooksHubWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            IEnumerable<Category> categoryObj = _unitOfWork.Category.GetAll();
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
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                //ModelState.AddModelError("CustomError", "Display Order Should Not Be The Exact Name");
                ModelState.AddModelError("Name", "Display Order Should Not Be The Exact Name");
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Category created successfully.";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        //Get
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var objFormDb = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == id);
            if (objFormDb == null)
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
                _unitOfWork.Category.Update(obj);
                _unitOfWork.Save();
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

            var objFormDb = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == id);
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
            var obj = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.Category.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Category deleted successfully.";
            return RedirectToAction("Index");
        }
    }
}
