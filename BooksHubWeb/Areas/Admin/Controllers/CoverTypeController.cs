using BooksHub.DataAccess.Repository.IRepository;
using BooksHub.Models;
using BooksHubWeb.Data;
using BooksHubWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace BooksHubWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CoverTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            IEnumerable<CoverType> coverTypeObj = _unitOfWork.CoverType.GetAll();
            return View(coverTypeObj);
        }

        //Get
        public IActionResult Create()
        {

            return View();
        }

        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CoverType obj)
        {
            //if (obj.Name == obj.Name.ToString())
            //{
            //    //ModelState.AddModelError("CustomError", "Display Order Should Not Be The Exact Name");
            //    ModelState.AddModelError("Name", "Display Order Should Not Be The Exact Name");
            //}
            if (ModelState.IsValid)
            {
                _unitOfWork.CoverType.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "CoverType created successfully.";
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

            var objFormDb = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id);
            if (objFormDb == null)
            {
                return NotFound();
            }

            return View(objFormDb);
        }

        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CoverType obj)
        {
            //if (obj.Name == obj.Name.ToString())
            //{
            //    //ModelState.AddModelError("CustomError", "Display Order Should Not Be The Exact Name");
            //    ModelState.AddModelError("Name", "Display Order Should Not Be The Exact Name");
            //}
            if (ModelState.IsValid)
            {
                _unitOfWork.CoverType.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "CoverType updated successfully.";
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

            var objFormDb = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id);
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
            var obj = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.CoverType.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "CoverType deleted successfully.";
            return RedirectToAction("Index");
        }
    }
}
