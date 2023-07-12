using BooksHub.DataAccess.Repository.IRepository;
using BooksHub.Models;
using BooksHubWeb.Data;
using BooksHubWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BooksHub.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db= db;    
        }
        
        public void Update(Product product)
        {
          var objFromDb = _db.Products.FirstOrDefault(u=>u.Id==product.Id);
            if (objFromDb != null)
            {
                objFromDb.Title= product.Title;
                objFromDb.ISBN= product.ISBN;
                objFromDb.Price= product.Price;
                objFromDb.Price100= product.Price100;
                objFromDb.Price50= product.Price50;
                objFromDb.Description= product.Description;
                objFromDb.Author= product.Author;
                objFromDb.CategoryId= product.CategoryId;
                objFromDb.CoverTypeId= product.CoverTypeId;
                if (objFromDb.ImageUrl != null)
                {
                    objFromDb.ImageUrl= product.ImageUrl;
                }
            }
        }
    }
}
