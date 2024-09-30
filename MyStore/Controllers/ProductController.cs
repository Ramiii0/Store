using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MyStore.Data;
using MyStore.DTO;
using MyStore.Mappers;
using Store.DataAccess.Interface;
using Store.DataAccess.Repository;
using Store.Models.Models;

namespace MyStore.Controllers
{
    [Route("/admin/[controller]/{action=Index}/{id?}")]
    [Authorize(Roles ="admin")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbCotext _context;
        private readonly IProductRepository _productRepository;
        
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly int pagesize = 5;


        public ProductController(IProductRepository productRepository, ApplicationDbCotext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _productRepository = productRepository;
        }
        public async Task<IActionResult> Index(int pageindex,string? search,string? column,string? orderBy)
        {
           var products = await _productRepository.GetAll(pageindex, search, column, orderBy);
            ViewData["PageIndex"] = pageindex;
            ViewData["TotalPages"] = ProductRepository.TotalPage;
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
                //_productRepository.Add(Product);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(dto);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productRepository.GetOne(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(CreateProductDto dto,int id)
        {
            var product = await _productRepository.GetOne(id);
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
            

            await _productRepository.Delete(id);
            return RedirectToAction("Index");
        }

    }
}
