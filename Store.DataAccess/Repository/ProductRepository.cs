using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MyStore.Data;
using Store.DataAccess.Interface;
using Store.Models.Models;
using Microsoft.AspNetCore.Hosting;



namespace Store.DataAccess.Repository
{

    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbCotext _db;
       //  private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly int pagesize = 5;

        public static int  TotalPage=0;
        public ProductRepository(ApplicationDbCotext applicationDbCotext)
        {
            _db = applicationDbCotext;
            
        }
        public async Task<List<Product>> GetAll(int pageindex, string? search, string? column, string? orderBy)
        {
            IQueryable<Product> query = _db.Products;
            if (search != null)
            {
                query = query.Where(x => x.Name.Contains(search) || x.Brand.Contains(search));
            }
            string[] validcolumn = { "CreatedAt", "Name", "Category", "Brand", "Price" };
            string[] validorder = { "desc", "asc" };
            if (!validcolumn.Contains(column))
            {
                column = "CreatedAt";
            }
            if (!validorder.Contains(orderBy))
            {
                orderBy = "desc";
            }
            if (column == "Name")
            {
                if (orderBy == "asc")
                {
                    query = query.OrderBy(x => x.Name);
                }
                else
                {
                    query = query.OrderByDescending(x => x.Name);
                }
            }
            else if (column == "Brand")
            {
                if (orderBy == "asc")
                {
                    query = query.OrderBy(x => x.Brand);
                }
                else
                {
                    query = query.OrderByDescending(x => x.Brand);
                }
            }
            else if (column == "Category")
            {
                if (orderBy == "asc")
                {
                    query = query.OrderBy(x => x.Category);
                }
                else
                {
                    query = query.OrderByDescending(x => x.Category);
                }
            }
            else if (column == "Price")
            {
                if (orderBy == "asc")
                {
                    query = query.OrderBy(x => x.Price);
                }
                else
                {
                    query = query.OrderByDescending(x => x.Price);
                }
            }
            else if (column == "CreatedAt")
            {
                if (orderBy == "asc")
                {
                    query = query.OrderBy(x => x.CreatedAt);
                }
                else
                {
                    query = query.OrderByDescending(x => x.CreatedAt);
                }
            }
            if (pageindex < 1)
            {
                pageindex = 1;

            }
            decimal count = query.Count();
            int totalpages = (int)Math.Ceiling(count / pagesize);
            TotalPage = totalpages;
            query = query.Skip((pageindex - 1) * pagesize).Take(pagesize);
            var products = query.ToList();
            return products;
        }

       public async Task<Product> GetOne(int id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product == null) { return null; }
            return product;
        }

       public void Add(Product product, IFormFile? file)
        {
           /* if (file != null)
            {
                //string wwwRootPath = _webHostEnvironment.WebRootPath;
                string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
             //   string productPath = Path.Combine(wwwRootPath, @"products");
                using (var filestream = new FileStream(Path.Combine(productPath, filename), FileMode.Create))
                {
                    file.CopyTo(filestream);
                };
                product.Imagefile = filename;
            }*/
            

        }
    }
}
