
using MyStore.Data;
using Store.Models.Models;
using System.Text.Json;

namespace MyStore.DTO
{
    public class CartHelper
    {
        public static Dictionary<int, int> GetCart(HttpRequest request, HttpResponse response)
        {
            string cookieValue = request.Cookies["shopping_cart"] ?? "";
            try
            {
                var cart= System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(cookieValue));
                Console.WriteLine(" cart is "+cart);
                var d = JsonSerializer.Deserialize<Dictionary<int,int>>(cart);
                if (d != null)
                {
                    return d;
                }
            }
            catch (Exception ex)
            {

            }
            if(cookieValue.Length > 0)
            {
                response.Cookies.Delete("shopping_cart");
            }
            return new Dictionary<int, int>();

        }
        public static int GetCartSize(HttpRequest request, HttpResponse response)
        {
            int cartSize = 0;
            var cartDictionary= GetCart(request, response);
            foreach (var item in cartDictionary)
            {
                cartSize += item.Value;
            }
            return cartSize;
        }
        public static List<OrderItem> GetOrderItems(HttpRequest request, HttpResponse response,ApplicationDbCotext applicationDbCotext)
        {
            var cartItem= new List<OrderItem>();
            var cartDictionary= GetCart(request, response);
            foreach (var item in cartDictionary)
            {
                int productId = item.Key;
                int quantity = item.Value;
              var  product= applicationDbCotext.Products.Find(productId);
                if (product == null) { continue; }
                var items = new OrderItem
                {
                    Quantity=quantity,
                    Product=product,
                    UnitPrice= product.Price,

                };
                cartItem.Add(items);

            }
            return cartItem;

        }
        public static decimal? GetSubTotal(List<OrderItem> items)
        {
            decimal? total = 0;
            foreach (var item in items)
            {
                total += item.Quantity * item.UnitPrice;

            }
            return total;
        }


    }
}
