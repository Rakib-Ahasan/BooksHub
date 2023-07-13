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
    public class CoverTypeRepository : Repository<CoverType>, ICoverTypeRepository
    {
        private readonly ApplicationDbContext _db;
        public CoverTypeRepository(ApplicationDbContext db) : base(db)
        {
            _db= db;    
        }
        
        public void Update(CoverType coverType)
        {
           _db.CoverTypes.Update(coverType);
        }
    }
}
