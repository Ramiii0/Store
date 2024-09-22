using System.ComponentModel.DataAnnotations.Schema;

namespace Store.Models.Models
{
    [Table("OrderItems")]
    public class OrderItem
    {
        public int Id { get; set; }
        public int? Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
        public Product Product { get; set; }
    }
}
