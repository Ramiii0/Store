using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyStore.Data;
using MyStore.DTO;
using Store.Models.Models;

namespace MyStore.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbCotext _applicationDbCotext;
        private readonly UserManager<AppUser> _userManager;
        private readonly decimal ShppingFee;
        public CartController(ApplicationDbCotext applicationDbCotext,UserManager<AppUser> userManager,IConfiguration configuration)
        {
            _applicationDbCotext = applicationDbCotext;
            _userManager = userManager;
            ShppingFee = configuration.GetValue<decimal>("Cartsitings:ShippingFee");
        }
        public IActionResult Index()
        {
            List<OrderItem> cartitem = CartHelper.GetOrderItems(Request, Response,_applicationDbCotext);
            decimal? total = CartHelper.GetSubTotal(cartitem);
            ViewBag.Total = total + ShppingFee;  
            ViewBag.CartItems = cartitem;
            ViewBag.SubTotal = total;
            ViewBag.ShippingFee=ShppingFee;

   
            return View();
        }
        [Authorize]
        [HttpPost]
        public IActionResult Index(CheckOutDto checkOutDto)
        {
            List<OrderItem> cartitem = CartHelper.GetOrderItems(Request, Response, _applicationDbCotext);
            decimal? total = CartHelper.GetSubTotal(cartitem);
            ViewBag.Total = total + ShppingFee;
            ViewBag.CartItems = cartitem;
            ViewBag.SubTotal = total;
            ViewBag.ShippingFee = ShppingFee;
            if (!ModelState.IsValid)
            {
                return View(checkOutDto);
            }
            if (cartitem.Count == 0)
            {
                ViewBag.ErrorMessage = "Your cart is empty";
                return View(checkOutDto);
            }
            TempData["DeliveryAddress"] = checkOutDto.DeliveryAddress;
            TempData["PaymentMethod"]=checkOutDto.PaymentMethod;

            return RedirectToAction("Confirm");
        }
        public IActionResult Confirm()
        {
            List<OrderItem> cartitem = CartHelper.GetOrderItems(Request, Response, _applicationDbCotext);
            decimal? total = CartHelper.GetSubTotal(cartitem) + ShppingFee;
            int? cartSize = 0;
            foreach (var item in cartitem)
            {
                cartSize += item.Quantity;
                
            }
            string deliveryaddres = TempData["DeliveryAddress"] as string ?? "";
           string paymentmethod= TempData["PaymentMethod"] as string ?? "";
            TempData.Keep();
            if (cartSize == 0 || deliveryaddres.Length == 0||paymentmethod.Length == 0)
            {
                return RedirectToAction("Index","Home");
            }
            ViewBag.DeliveryAddress=deliveryaddres;
            ViewBag.PaymentMethod=paymentmethod;
            ViewBag.Total=total;
            ViewBag.CartSize=cartSize;

            return View(); 
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Confirm(string? s)
        {
            var cartItem= CartHelper.GetOrderItems(Request,Response, _applicationDbCotext);
            string deliveryaddres = TempData["DeliveryAddress"] as string ?? "";
            string paymentmethod = TempData["PaymentMethod"] as string ?? "";
            TempData.Keep();
            if(cartItem.Count == 0)
            {

            return RedirectToAction("Index","Home");
            }
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var order = new Order
            {
                ClientId = user.Id,
                Items = cartItem,
                ShippingFee=ShppingFee,
                DelievryAdress=deliveryaddres,
                PaymentDetails=paymentmethod,
                PaymentStatus="pending",
                OrderStatus="pending",
                PaymentMethod=paymentmethod,
                
                CreatedAt= DateTime.Now,
               

            };
            _applicationDbCotext.Orders.Add(order);
            _applicationDbCotext.SaveChanges();
            Response.Cookies.Delete("shopping_cart");
            ViewBag.SuccessMessage = "Order Submitted Successfuly";
            return View();

        }


        }
    
}