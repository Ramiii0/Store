using Microsoft.EntityFrameworkCore;

namespace Store.Models.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string ClientId { get; set; }
        public AppUser Client { get; set; }
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
        [Precision(16,2 )]
        public decimal ShippingFee { get; set; }
        public string DelievryAdress { get; set; }
        public string   PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }
        public string PaymentDetails { get; set; }
        public string OrderStatus { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }
}
