using Microsoft.AspNetCore.Http;
using Store.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.DataAccess.Interface
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAll(int pageindex, string? search, string? column, string? orderBy);
        Task<Product> GetOne(int id);
        void Add(Product product, IFormFile? file);
        


    }
}
