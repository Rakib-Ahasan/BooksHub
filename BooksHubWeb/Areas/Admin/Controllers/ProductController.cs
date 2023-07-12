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
		//GET
		public IActionResult Upsert(int? id)
		{
			ProductViewModel productVM = new()
			{
				Product = new(),
				CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
				{
					Text = i.Name,
					Value = i.Id.ToString()
				}),
				CoverTypeList = _unitOfWork.CoverType.GetAll().Select(i => new SelectListItem
				{
					Text = i.Name,
					Value = i.Id.ToString()
				}),
			};

			if (id == null || id == 0)
			{
				//create product
				//ViewBag.CategoryList = CategoryList;
				//ViewData["CoverTypeList"] = CoverTypeList;
				return View(productVM);
			}
			else
			{
				//update product 
				productVM.Product = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);
				return View(productVM);
			}


		}

		//POST
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Upsert(ProductViewModel obj, IFormFile file)
		{

			if (ModelState.IsValid)
			{
				string wwwRootPath = _webHostEnvironment.WebRootPath;
				if (file != null)
				{
					string fileName = Guid.NewGuid().ToString();
					var uploads = Path.Combine(wwwRootPath, @"images\products");
					var extension = Path.GetExtension(file.FileName);

					if (obj.Product.ImageUrl != null)
					{
						var oldImagePath = Path.Combine(wwwRootPath, obj.Product.ImageUrl.TrimStart('\\'));
						if (System.IO.File.Exists(oldImagePath))
						{
							System.IO.File.Delete(oldImagePath);
						}
					}
					using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
					{
						file.CopyTo(fileStreams);
					}

					obj.Product.ImageUrl = @"\images\products\" + fileName + extension;
				}

				if (obj.Product.Id == 0)
				{
					_unitOfWork.Product.Add(obj.Product);
				}
				else
				{
					_unitOfWork.Product.Update(obj.Product);
				}

				_unitOfWork.Save();
				TempData["success"] = "Product created successfully";
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
