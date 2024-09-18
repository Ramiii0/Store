using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MyStore.DTO
{
    public class CreateProductDto
    {
        public string? Name { get; set; }
        [MaxLength(100)]
        public string? Brand { get; set; }
        [MaxLength(100)]
        public string? Category { get; set; }
        [Precision(16, 2)]
        public decimal? Price { get; set; }
        public IFormFile? Imagefile { get; set; }
        public string? Description { get; set; }
    }
}
