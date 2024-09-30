using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyStore.Data;
using MyStore.DTO;
using Store.DataAccess.Interface;
using Store.DataAccess.Repository;
using Store.Models.Models;

namespace MyStore.Controllers
{
    [Authorize]
    public class StoreController : Controller
    {
        
        private readonly ApplicationDbCotext _context;
        private readonly IStoreRepository _repository;
        private readonly int pagesize = 8;
        public StoreController(ApplicationDbCotext cotext, IStoreRepository repository)
        {
            _context = cotext;
            _repository = repository;
            
        }
        public async Task<IActionResult> Index(int pageIndex,string? search,string brand,string category,string sort)
        {
            var products= await _repository.GetAll(pageIndex,search,brand,category,sort);
            ViewData["PageIndex"] = pageIndex;
            ViewData["TotalPages"] = StoreRepository.TotalPages;
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
        public async Task<IActionResult> Details(int id)
        {
            var product= await _repository.GetOne(id);
            if (product == null)
            {
                return View("Error");

            }
            return View(product);
        }
    }
}
