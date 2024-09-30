using Store.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.DataAccess.Interface
{
    public  interface IStoreRepository
    {
        Task<List<Product>> GetAll(int pageIndex, string? search, string brand, string category, string sort);
        Task<Product> GetOne(int id);
        
    }
}
