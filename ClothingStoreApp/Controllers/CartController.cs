// Controllers/CartController.cs
using ClothingStore.Core.Entities;
using ClothingStore.Core.Interfaces;
using ClothingStoreApp.Models;
using ClothingStoreApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ClothingStoreApp.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IProductRepository _productRepo;

        public CartController(ICartService cartService, IProductRepository productRepo)
        {
            _cartService = cartService;
            _productRepo = productRepo;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!string.IsNullOrEmpty(userId))
            {
                var dbItems = await _cartService.GetDbCartItemsAsync();

                return View(new CartPageViewModel
                {
                    DbCart = dbItems
                });
            }
            else
            {
                var sessionItems = await _cartService.GetSessionCartAsync();

                return View(new CartPageViewModel
                {
                    SessionCart = sessionItems
                });
            }
        }

        // Allow GET for simple link, or use POST forms for CSRF-safe approach
        [HttpGet]
        public async Task<IActionResult> Add(int id)
        {
            await _cartService.AddToCartAsync(id, 1);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Remove(int id)
        {
            await _cartService.RemoveFromCartAsync(id);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Increase(int id)
        {
            await _cartService.IncreaseAsync(id);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Decrease(int id)
        {
            await _cartService.DecreaseAsync(id);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Clear()
        {
            await _cartService.ClearCartAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Checkout()
        {
            // 👤 Check login status
            if (!User.Identity.IsAuthenticated)
            {
                TempData["CheckoutError"] = "Please login to continue with checkout.";
                return RedirectToAction("Login", "Account", new { returnUrl = "/Cart/Checkout" });
            }

            // ✔ User is logged in → use their DB cart
            var dbCart = await _cartService.GetDbCartItemsAsync();

            if (dbCart == null || !dbCart.Any())
                return RedirectToAction("Index", "Cart");

            ViewBag.Cart = dbCart.Select(x => new CartViewModel
            {
                ProductId = x.ProductId,
                Name = x.Product.Name,
                Price = x.Product.Price,
                Quantity = x.Quantity,
                ImageUrl = x.Product.ImageUrl
            }).ToList();

            ViewBag.Total = dbCart.Sum(x => x.Product.Price * x.Quantity);

            return View(new CheckoutViewModel());
        }


        public IActionResult Success(int id)
        {
            ViewBag.OrderId = id;
            return View();
        }
    }
}
