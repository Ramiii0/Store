using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MyStore.Data;
using MyStore.DTO;
using MyStore.Mappers;
using Store.Models.Models;

namespace MyStore.Controllers
{
    [Route("/admin/[controller]/{action=Index}/{id?}")]
    [Authorize(Roles ="admin")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbCotext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly int pagesize = 5;


        public ProductController(ApplicationDbCotext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index(int pageindex,string? search,string? column,string? orderBy)
        {
            IQueryable<Product> query=  _context.Products;
            if (search != null)
            {
                query= query.Where(x=>x.Name.Contains(search) || x.Brand.Contains(search));
            }
            string[] validcolumn = {"CreatedAt", "Name", "Category", "Brand", "Price" };
            string[] validorder = {"desc","asc"};
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
                    query= query.OrderBy(x=>x.Name);    
                }
                else
                {
                    query=query.OrderByDescending(x=>x.Name);
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
            } else if (column == "Price")
            {
                if (orderBy == "asc")
                {
                    query = query.OrderBy(x => x.Price);
                }
                else
                {
                    query =query.OrderByDescending(x => x.Price);
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
            decimal count= query.Count();
            int totalpages = (int)Math.Ceiling(count/pagesize);
            query=query.Skip((pageindex -1) * pagesize).Take(pagesize);
            var products = query.ToList();
            ViewData["PageIndex"] = pageindex;
            ViewData["TotalPages"] = totalpages;
            ViewData["Search"] = search ?? "";
            ViewData["column"]=column;
            ViewData["orderBy"]=orderBy;


            return View(products);
        }
        public  IActionResult Create()
        {
          return View();

        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateProductDto dto)
        {
           
            if (ModelState.IsValid)
            {
                var Product= dto.ToCreateProduct();
                if (dto.Imagefile != null)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(dto.Imagefile.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"products");
                    using (var filestream = new FileStream(Path.Combine(productPath, filename), FileMode.Create))
                    {
                        dto.Imagefile.CopyTo(filestream);
                    }; 
                    Product.Imagefile =  filename;
                }
                await _context.Products.AddAsync(Product);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(dto);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _context.Products.SingleOrDefaultAsync(x => x.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(CreateProductDto dto,int id)
        {
            var product = await _context.Products.SingleOrDefaultAsync(x => x.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                //var product= dto.ToCreateProduct();
                if (dto.Imagefile != null)
                {

                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(dto.Imagefile.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"products");
                    using (var filestream = new FileStream(Path.Combine(productPath, filename), FileMode.Create))
                    {
                        dto.Imagefile.CopyTo(filestream);
                    };
                    string oldImagepath = _webHostEnvironment.WebRootPath + "/products/" + product.Imagefile;
                    product.Imagefile = filename;
                    
                    System.IO.File.Delete(oldImagepath);

                }
                if (dto.Name !=null)
                {
                    product.Name = dto.Name;

                }
                if (dto.Price != null)
                {
                    product.Price = dto.Price;

                }
                if (dto.Description != null)
                {
                    product.Description = dto.Description;

                }
                if (dto.Brand != null)
                {
                    product.Brand = dto.Brand;

                }
                if (dto.Category != null)
                {
                    product.Category = dto.Category;

                }
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");



            }
            return View(dto);

        }
        public async Task<IActionResult> Delete(int id)
        {
            var product= await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

    }
}
