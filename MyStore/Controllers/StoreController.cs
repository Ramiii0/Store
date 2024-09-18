using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyStore.Data;
using MyStore.DTO;
using MyStore.Models;

namespace MyStore.Controllers
{
    [Authorize]
    public class StoreController : Controller
    {
        
        private readonly ApplicationDbCotext _context;
        private readonly int pagesize = 8;
        public StoreController(ApplicationDbCotext cotext)
        {
            _context = cotext;
            
        }
        public IActionResult Index(int pageIndex,string? search,string brand,string category,string sort)
        {
            IQueryable<Product> query =  _context.Products;
           
            if (search != null && search.Length >0)
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
            } else if (sort =="price_desc")
            {
                query = query.OrderByDescending(x => x.Price);

            } else
            {
                query = query.OrderByDescending(x => x.CreatedAt);
            }

            decimal count = query.Count();
            int totalpages = (int)Math.Ceiling(count / pagesize);
            query = query.Skip((pageIndex - 1) * pagesize).Take(pagesize);
            var products = query.ToList();
            ViewData["PageIndex"] = pageIndex;
            ViewData["TotalPages"] = totalpages;
            ViewData["Search"] = search ?? "";
            ViewBag.Product = products;
            var searchModel = new StoreVm()
            {
                search = search,
                brand = brand,
                sort = sort,
                category = category,
                

            };
            return View(searchModel);
        }
        public IActionResult Details(int id)
        {
            var product= _context.Products.Find(id);
            if (product == null)
            {
                return View("Error");

            }
            return View(product);
        }
    }
}
