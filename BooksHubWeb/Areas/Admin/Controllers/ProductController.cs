using BooksHub.DataAccess.Repository.IRepository;
using BooksHub.Models;
using BooksHub.Models.ViewModels;
using BooksHubWeb.Data;
using BooksHubWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace BooksHubWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;   
        }
        public IActionResult Index()
        {
            IEnumerable<CoverType> coverTypeObj = _unitOfWork.CoverType.GetAll();
            return View(coverTypeObj);
        }


        //Get
        public IActionResult Upsert(int? id)
        {
            ProductViewModel productViewModel = new()
            {
                Product = new(),
                CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem {

                    Text = i.Name,
                    Value = i.Id.ToString(),
                }),
                CoverTypeList = _unitOfWork.CoverType.GetAll().Select(i => new SelectListItem
                {

                    Text = i.Name,
                    Value = i.Id.ToString(),
                })
            };
            
            if (id == null || id == 0)
            {
                //Create Product

                //ViewBag.CoverTypeList = CategoryList;
                //ViewData["CoverTypeList"] = CoverTypeList;
                return View(productViewModel);
            }
            else
            {
                //Update Product
            }

            
            return View(productViewModel);
        }

        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductViewModel product, IFormFile file)
        {
            //if (obj.Name == obj.Name.ToString())
            //{
            //    //ModelState.AddModelError("CustomError", "Display Order Should Not Be The Exact Name");
            //    ModelState.AddModelError("Name", "Display Order Should Not Be The Exact Name");
            //}
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if(file != null) 
                {
                    string fileName = Guid.NewGuid().ToString();
                    var upload = Path.Combine(wwwRootPath, @"images\products");
                    var extensions = Path.GetExtension(file.FileName);

                    using(var fileStream = new FileStream(Path.Combine(upload,fileName + extensions),FileMode.Create)) 
                    {
                        file.CopyTo(fileStream);
                    }
                    product.Product.ImageUrl = @"images\products" + fileName + extensions;
                }
                _unitOfWork.Product.Add(product.Product);
                _unitOfWork.Save();
                TempData["success"] = "Product created successfully.";
                return RedirectToAction("Index");
            }
            return View();
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
