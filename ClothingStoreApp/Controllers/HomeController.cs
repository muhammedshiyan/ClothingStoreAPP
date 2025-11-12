using ClothingStore.Core.Interfaces;
using ClothingStoreApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ClothingStoreApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductRepository _repo;
        public HomeController(ILogger<HomeController> logger, IProductRepository repo)
        {
            _logger = logger;
            _repo = repo;
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}
        //public async Task<IActionResult> Index()
        //{
        //    var products = await _repo.GetAllAsync();
        //    return View(products);
        //}

        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 5;

            var allProducts = await _repo.GetPagedAsync(page, pageSize);
            var totalProducts = allProducts.Count();

            var products = allProducts
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .ToList();

            var vm = new PagedProductsViewModel
            {
                Products = products,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(totalProducts / (double)pageSize)
            };

            return View(vm);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
