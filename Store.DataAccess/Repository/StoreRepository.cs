using Microsoft.EntityFrameworkCore;
using MyStore.Data;
using Store.DataAccess.Interface;
using Store.Models.Models;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.DataAccess.Repository
{
    public class StoreRepository : IStoreRepository
    {
        private readonly ApplicationDbCotext _context;
        private readonly int pagesize = 8;
        public static int TotalPages = 0;
        public StoreRepository(ApplicationDbCotext cotext)
        {
            _context = cotext;
            

        }
       

        public async Task<List<Product>> GetAll(int pageIndex, string? search, string brand, string category, string sort)
        {
            IQueryable<Product> query = _context.Products;

            if (search != null && search.Length > 0)
            {
                query = query.Where(x => x.Name.Contains(search));
            }
            if (brand != null && brand.Length > 0)
            {
                query = query.Where(x => x.Brand.Contains(brand));
            }
            if (category != null && category.Length > 0)
            {
                query = query.Where(x => x.Category.Contains(category));
            }
            if (pageIndex < 1)
            {
                pageIndex = 1;

            }
            if (sort == "price_asc")
            {
                query = query.OrderBy(x => x.Price);
            }
            else if (sort == "price_desc")
            {
                query = query.OrderByDescending(x => x.Price);

            }
            else
            {
                query = query.OrderByDescending(x => x.CreatedAt);
            }

            decimal count = query.Count();
            int totalpages = (int)Math.Ceiling(count / pagesize);
            TotalPages = totalpages;
            query = query.Skip((pageIndex - 1) * pagesize).Take(pagesize);
            var products = query.ToList();
            return products;
        }

        public async Task<Product> GetOne(int id)
        {
            var product = await _context.Products.FindAsync(id);
            return product;
        }
    }
}
