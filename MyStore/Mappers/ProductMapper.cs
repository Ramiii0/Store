using Microsoft.CodeAnalysis.CSharp.Syntax;
using MyStore.DTO;
using MyStore.Models;

namespace MyStore.Mappers
{
    public static class ProductMapper
    {
        public static Product ToCreateProduct(this CreateProductDto model)
        {
            return new Product
            {
                Name = model.Name,
                Description = model.Description,
                Brand = model.Brand,
                Category = model.Category,
                Price = model.Price,
                CreatedAt= DateTime.Now,

            };
        }

    }
}
